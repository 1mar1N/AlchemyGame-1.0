using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Media;
using System.Text;
namespace AlchemyGame.Services;
using System.Threading;
using System.Threading.Tasks;

public static class SoundService
{
    public static bool Enabled { get; set; } = true;


    public static void PlaySpawn()
    {
        if (!Enabled) return;
        PlayAsync(() => PlayTone(392, 60, 0.25)); 
    }

    public static void PlayCraft()
    {
        if (!Enabled) return;
        PlayAsync(() =>
        {
            PlayTone(440, 90, 0.40);  
            Thread.Sleep(70);
            PlayTone(554, 110, 0.40); 
        });
    }

    public static void PlayNewDiscovery()
    {
        if (!Enabled) return;
        PlayAsync(() =>
        {
            PlayTone(523, 100, 0.50);  
            Thread.Sleep(75);
            PlayTone(659, 100, 0.50); 
            Thread.Sleep(75);
            PlayTone(784, 140, 0.55); 
            Thread.Sleep(110);
            PlayTone(1047, 200, 0.45); 
        });
    }


    private static void PlayAsync(Action action)
        => Task.Run(() =>
        {
            try { action(); }
            catch {}
        });

    private static void PlayTone(int frequencyHz, int durationMs, double volume)
    {
        using var stream = BuildWav(frequencyHz, durationMs, volume);
        using var player = new SoundPlayer(stream);
        player.PlaySync();
    }

    private static MemoryStream BuildWav(int freq, int durationMs, double volume)
    {
        const int sampleRate = 44100;
        int totalSamples = sampleRate * durationMs / 1000;

        var stream = new MemoryStream(44 + totalSamples * 2);
        using var w = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: true);

        w.Write(Encoding.ASCII.GetBytes("RIFF"));
        w.Write(36 + totalSamples * 2);          
        w.Write(Encoding.ASCII.GetBytes("WAVE"));

        w.Write(Encoding.ASCII.GetBytes("fmt "));
        w.Write(16);                              
        w.Write((short)1);                        
        w.Write((short)1);                        
        w.Write(sampleRate);                     
        w.Write(sampleRate * 2);                  
        w.Write((short)2);                       
        w.Write((short)16);                       

        w.Write(Encoding.ASCII.GetBytes("data"));
        w.Write(totalSamples * 2);

        double attackEnd  = totalSamples * 0.10;
        double sustainEnd = totalSamples * 0.70;

        for (int i = 0; i < totalSamples; i++)
        {
            double env = i < attackEnd
                ? i / attackEnd
                : i > sustainEnd
                    ? 1.0 - (i - sustainEnd) / (totalSamples - sustainEnd)
                    : 1.0;

            double sample = Math.Sin(2 * Math.PI * freq * i / sampleRate) * env * volume;
            w.Write((short)(sample * short.MaxValue));
        }

        stream.Position = 0;
        return stream;
    }
}
