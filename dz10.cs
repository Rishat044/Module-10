using System;

class TV
{
    public void On()
    {
        Console.WriteLine("Телевизор включен.");
    }

    public void Off()
    {
        Console.WriteLine("Телевизор выключен.");
    }

    public void SetChannel(string channel)
    {
        Console.WriteLine($"Канал переключен на {channel}.");
    }
}

class AudioSystem
{
    public void On()
    {
        Console.WriteLine("Аудиосистема включена.");
    }

    public void Off()
    {
        Console.WriteLine("Аудиосистема выключена.");
    }

    public void SetVolume(int level)
    {
        Console.WriteLine($"Громкость установлена на уровень {level}.");
    }
}

class DVDPlayer
{
    public void Play()
    {
        Console.WriteLine("DVD-проигрыватель начал воспроизведение.");
    }

    public void Pause()
    {
        Console.WriteLine("DVD-проигрыватель на паузе.");
    }

    public void Stop()
    {
        Console.WriteLine("DVD-проигрыватель остановлен.");
    }
}

class GameConsole
{
    public void On()
    {
        Console.WriteLine("Игровая консоль включена.");
    }

    public void StartGame(string game)
    {
        Console.WriteLine($"Игра '{game}' запущена.");
    }
}







class HomeTheaterFacade
{
    private TV tv;
    private AudioSystem audioSystem;
    private DVDPlayer dvdPlayer;
    private GameConsole gameConsole;

    public HomeTheaterFacade(TV tv, AudioSystem audioSystem, DVDPlayer dvdPlayer, GameConsole gameConsole)
    {
        this.tv = tv;
        this.audioSystem = audioSystem;
        this.dvdPlayer = dvdPlayer;
        this.gameConsole = gameConsole;
    }

    public void WatchMovie()
    {
        Console.WriteLine("\nПодготовка к просмотру фильма:");
        tv.On();
        audioSystem.On();
        dvdPlayer.Play();
        audioSystem.SetVolume(5);
        tv.SetChannel("HDMI 1");
        Console.WriteLine("Фильм запущен!\n");
    }

    public void StopMovie()
    {
        Console.WriteLine("\nОстановка фильма:");
        dvdPlayer.Stop();
        audioSystem.Off();
        tv.Off();
        Console.WriteLine("Фильм остановлен и система выключена.\n");
    }

    public void PlayGame(string gameName)
    {
        Console.WriteLine("\nПодготовка к игре:");
        tv.On();
        gameConsole.On();
        audioSystem.On();
        tv.SetChannel("HDMI 2");
        audioSystem.SetVolume(7);
        gameConsole.StartGame(gameName);
        Console.WriteLine($"Игра '{gameName}' начата!\n");
    }

    public void StopGame()
    {
        Console.WriteLine("\nВыход из игры:");
        audioSystem.Off();
        tv.Off();
        Console.WriteLine("Игра остановлена и система выключена.\n");
    }

    public void ListenToMusic()
    {
        Console.WriteLine("\nПодготовка к прослушиванию музыки:");
        tv.On();
        audioSystem.On();
        tv.SetChannel("AUX");
        audioSystem.SetVolume(8);
        Console.WriteLine("Музыка включена!\n");
    }

    public void SetVolume(int level)
    {
        audioSystem.SetVolume(level);
        Console.WriteLine($"Громкость установлена на уровень {level}.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        TV tv = new TV();
        AudioSystem audioSystem = new AudioSystem();
        DVDPlayer dvdPlayer = new DVDPlayer();
        GameConsole gameConsole = new GameConsole();

        HomeTheaterFacade homeTheater = new HomeTheaterFacade(tv, audioSystem, dvdPlayer, gameConsole);

        homeTheater.WatchMovie();
        homeTheater.SetVolume(10);
        homeTheater.StopMovie();

        homeTheater.PlayGame("Super Mario");
        homeTheater.SetVolume(6);
        homeTheater.StopGame();

        homeTheater.ListenToMusic();
        homeTheater.SetVolume(4);
    }
}

///////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;

abstract class FileSystemComponent
{
    public string Name { get; protected set; }

    protected FileSystemComponent(string name)
    {
        Name = name;
    }

    public abstract void Display(int depth = 0);
    public abstract int GetSize();
}
class File : FileSystemComponent
{
    public int Size { get; private set; }

    public File(string name, int size) : base(name)
    {
        Size = size;
    }

    public override void Display(int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth * 2)}- Файл: {Name}, Размер: {Size} KB");
    }

    public override int GetSize()
    {
        return Size;
    }
}

class Directory : FileSystemComponent
{
    private List<FileSystemComponent> components = new List<FileSystemComponent>();

    public Directory(string name) : base(name) { }

    public void Add(FileSystemComponent component)
    {
        if (!components.Contains(component))
        {
            components.Add(component);
            Console.WriteLine($"Добавлено: {component.Name} в папку {Name}");
        }
    }

    public void Remove(FileSystemComponent component)
    {
        if (components.Contains(component))
        {
            components.Remove(component);
            Console.WriteLine($"Удалено: {component.Name} из папки {Name}");
        }
    }

    public override void Display(int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth * 2)}+ Папка: {Name}");
        foreach (var component in components)
        {
            component.Display(depth + 1);
        }
    }

    public override int GetSize()
    {
        int totalSize = 0;
        foreach (var component in components)
        {
            totalSize += component.GetSize();
        }
        return totalSize;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var file1 = new File("Документ.txt", 120);
        var file2 = new File("Фото.jpg", 850);
        var file3 = new File("Презентация.pptx", 300);
        var file4 = new File("Музыка.mp3", 4500);

        var rootDirectory = new Directory("Корневая папка");
        var documentsFolder = new Directory("Документы");
        var mediaFolder = new Directory("Медиа");

        documentsFolder.Add(file1);
        documentsFolder.Add(file3);

        mediaFolder.Add(file2);
        mediaFolder.Add(file4);

        rootDirectory.Add(documentsFolder);
        rootDirectory.Add(mediaFolder);

        Console.WriteLine("Структура файловой системы:\n");
        rootDirectory.Display();
        Console.WriteLine($"\nОбщий размер: {rootDirectory.GetSize()} KB");
    }
}