using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsAggregatorServices.Abstracts;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace NewsAggregatorServices.Implementations
{
    public class MyStemTextlemmatizer : ITextlemmatizer, IAsyncDisposable
    {
        const int _writingPause = 10;
        const int _killingMyStemPause = 500;

        readonly ILogger<MyStemTextlemmatizer> _logger;
        readonly IConfiguration _configuration;
        Process _myStem;

        public MyStemTextlemmatizer(IConfiguration configuration, ILogger<MyStemTextlemmatizer> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Dictionary<Guid, string[]>> GetLemmasFromTextsAsync(Dictionary<Guid, string> textById, CancellationToken cancellationToken)
        {
            try
            {
                StartMyStem();
                var idArray = textById.Keys.ToArray();
                var textsArray = textById.Values.ToArray();
                var lemmasTask = ReadLemmasFromMyStemAsync(idArray, cancellationToken);
                var errorsTask = ReadErrorsFromMyStemAsync(idArray, cancellationToken);
                await WriteToMyStemAsync(textsArray, cancellationToken);
                await _myStem.WaitForExitAsync(cancellationToken);
                await Task.WhenAll(lemmasTask, errorsTask);
                var errorsById = await errorsTask;
                return await lemmasTask;
            }
            catch (Exception)
            {
                _logger.LogError($"Error while trying to get lemmas from texts {_myStem.ProcessName}");
                throw new Exception("MyStem error");
            }
        }

        private string[] DeserializeLemmas(string? jsonLemmas)
        {
            if (jsonLemmas is null)
                return Array.Empty<string>();
            return JsonConvert.DeserializeObject<WordLemma[]>(jsonLemmas)
                .SelectMany(w => w.Analysis)
                .Select(a => a.Lex)
                .ToArray();
        }

        private void StartMyStem()
        {
            var mySteamPatch = _configuration.GetSection("MyStem:Patch").Value;
            var mySteamArguments = _configuration.GetSection("MyStem:Arguments").Value;
            if (mySteamPatch is null || mySteamArguments is null)
                throw new Exception("There are no settings for MySteam in the configuration");
            var startInfo = new ProcessStartInfo
            {
                FileName = mySteamPatch,
                Arguments = mySteamArguments,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardInputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8
            };
            _myStem = new Process { StartInfo = startInfo };
            _myStem.Start();
        }

        private async Task<Dictionary<Guid, string[]>> ReadLemmasFromMyStemAsync(Guid[] idArray, CancellationToken cancellationToken)
        {
            var lemmasById = new Dictionary<Guid, string[]>();
            using (var reader = _myStem.StandardOutput)
            {
                for (int i = 0; i < idArray.Length; i++)
                {
                    var jsonLemmas = await reader.ReadLineAsync(cancellationToken);
                    var lemmas = DeserializeLemmas(jsonLemmas);
                    lemmasById.Add(idArray[i], lemmas);
                }
            }
            return lemmasById;
        }

        private async Task<Dictionary<Guid, string>> ReadErrorsFromMyStemAsync(Guid[] idArray, CancellationToken cancellationToken)
        {
            var errorsById = new Dictionary<Guid, string>();
            using (var errorsReader = _myStem.StandardError)
            {
                for (int i = 0; i < idArray.Length; i++)
                {
                    var error = await errorsReader.ReadLineAsync(cancellationToken);
                    if (String.IsNullOrEmpty(error))
                        continue;
                    errorsById.Add(idArray[i], error);
                }
            }
            return errorsById;
        }

        private async Task WriteToMyStemAsync(string[] textsArray, CancellationToken cancellationToken)
        {
            using (var writer = _myStem.StandardInput)
            {
                foreach (var text in textsArray)
                {
                    await writer.WriteLineAsync(new StringBuilder(text), cancellationToken);
                    await Task.Delay(_writingPause, cancellationToken);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_myStem is null)
                return;
            _myStem.Kill();
            await Task.Delay(_killingMyStemPause);
            _myStem.Dispose();
        }

        private class WordLemma
        {
            public string Text { get; set; }
            public Analysis[] Analysis { get; set; }
        }
        private class Analysis
        {
            public string Lex { get; set; } 
        }
    }
}


