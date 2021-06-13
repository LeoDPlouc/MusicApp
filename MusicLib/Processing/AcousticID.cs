using AcoustID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicLib.Processing
{
    class AcousticID
    {
        public static string ComputeAcousticId(string path)
        {
            NAudio.Wave.AudioFileReader reader = new NAudio.Wave.AudioFileReader(path);

            byte[] buffer = new byte[reader.Length];
            reader.Read(buffer, 0, buffer.Length);
            short[] data = buffer.Select((byte b) =>
            {
                return Convert.ToInt16(b);
            }).ToArray();

            ChromaContext context = new ChromaContext();
            context.Start(reader.WaveFormat.SampleRate, reader.WaveFormat.Channels);
            context.Feed(data, data.Length);
            context.Finish();

            reader.Dispose();

            return context.GetFingerprint();
        }
    }
}
