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
    /// <summary>Выключить звук (для настроек)</summary>
    public static bool Enabled { get; set; } = true;

    // ── Публичные методы ─────────────────────────────────────────────────

    /// <summary>Мягкий pop при добавлении элемента на поле</summary>
    public static void PlaySpawn()
    {
        if (!Enabled) return;
        PlayAsync(() => PlayTone(392, 60, 0.25)); // G4, тихий, короткий
    }

    /// <summary>Двухнотный звук при успешном крафте</summary>
    public static void PlayCraft()
    {
        if (!Enabled) return;
        PlayAsync(() =>
        {
            PlayTone(440, 90, 0.40);  // A4
            Thread.Sleep(70);
            PlayTone(554, 110, 0.40); // C#5
        });
    }

    /// <summary>Торжественный аккорд при открытии нового элемента</summary>
    public static void PlayNewDiscovery()
    {
        if (!Enabled) return;
        PlayAsync(() =>
        {
            PlayTone(523, 100, 0.50);  // C5
            Thread.Sleep(75);
            PlayTone(659, 100, 0.50);  // E5
            Thread.Sleep(75);
            PlayTone(784, 140, 0.55);  // G5
            Thread.Sleep(110);
            PlayTone(1047, 200, 0.45); // C6 — финальная высокая нота
        });
    }

    // ── Внутренние методы ─────────────────────────────────────────────────

    /// <summary>Запускает воспроизведение в пуле потоков, не блокируя UI</summary>
    private static void PlayAsync(Action action)
        => Task.Run(() =>
        {
            try { action(); }
            catch { /* игнорируем ошибки звука */ }
        });

    /// <summary>Воспроизводит один синусоидальный тон</summary>
    private static void PlayTone(int frequencyHz, int durationMs, double volume)
    {
        using var stream = BuildWav(frequencyHz, durationMs, volume);
        using var player = new SoundPlayer(stream);
        player.PlaySync();
    }

    /// <summary>
    /// Генерирует WAV-файл в памяти.
    /// Формат: PCM 16-bit, моно, 44100 Гц.
    /// Огибающая: атака 10% + поддержка 60% + спад 30%.
    /// </summary>
    private static MemoryStream BuildWav(int freq, int durationMs, double volume)
    {
        const int sampleRate = 44100;
        int totalSamples = sampleRate * durationMs / 1000;

        var stream = new MemoryStream(44 + totalSamples * 2);
        using var w = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: true);

        // ── RIFF-заголовок ────────────────────────────────────────────────
        w.Write(Encoding.ASCII.GetBytes("RIFF"));
        w.Write(36 + totalSamples * 2);          // размер всего файла − 8
        w.Write(Encoding.ASCII.GetBytes("WAVE"));

        // ── fmt-чанк (описание формата) ───────────────────────────────────
        w.Write(Encoding.ASCII.GetBytes("fmt "));
        w.Write(16);                              // размер чанка fmt
        w.Write((short)1);                        // PCM = 1
        w.Write((short)1);                        // каналы: моно
        w.Write(sampleRate);                      // частота дискретизации
        w.Write(sampleRate * 2);                  // байт в секунду
        w.Write((short)2);                        // блок выравнивания
        w.Write((short)16);                       // бит на сэмпл

        // ── data-чанк (сами сэмплы) ───────────────────────────────────────
        w.Write(Encoding.ASCII.GetBytes("data"));
        w.Write(totalSamples * 2);

        double attackEnd  = totalSamples * 0.10;
        double sustainEnd = totalSamples * 0.70;

        for (int i = 0; i < totalSamples; i++)
        {
            // Огибающая: плавный старт и конец
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
