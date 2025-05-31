using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Whisper.net;

namespace ShengWen.Agent.Services
{
    public class AudioProcessingService
    {
        private readonly WhisperFactory _whisperFactory;

        public AudioProcessingService()
        {
            _whisperFactory = WhisperFactory.FromPath("ggml-large-v3-q5_0.bin");
        }

        public async Task<string> GenerateSrtAsync(byte[] audioData)
        {
            using var processor = _whisperFactory.CreateBuilder()
                .WithLanguage("auto")
                .Build();

            using var memoryStream = new MemoryStream(audioData);
            
            var segments = processor.ProcessAsync(memoryStream);
            var srtBuilder = new System.Text.StringBuilder();
            int index = 1;

            await foreach (var segment in segments)
            {
                srtBuilder.AppendLine($"{index++}");
                srtBuilder.AppendLine($"{segment.Start:hh\\:mm\\:ss\\,fff} --> {segment.End:hh\\:mm\\:ss\\,fff}");
                srtBuilder.AppendLine(segment.Text);
                srtBuilder.AppendLine();
            }

            return srtBuilder.ToString();
        }
    }
}
