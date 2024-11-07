using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PaperScoreTracker.Models;
using PaperScoreTracker.Services;
using System.Collections.ObjectModel;

namespace PaperScoreTracker.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly PlayerControler _playerControler;

    [ObservableProperty]
    private string _playerAlias;

    [ObservableProperty]
    private ObservableCollection<Player> _players;

    public MainViewModel(PlayerControler playerControler)
    {
        _playerControler = playerControler;

        Players = new ObservableCollection<Player>(_playerControler.GetAllPlayers());
    }

    [RelayCommand]
    private void Add()
    {
        var newPlayer = new Player(PlayerAlias);

        _playerControler.AddPlayer(newPlayer);

        Players.Add(newPlayer);

        PlayerAlias = string.Empty;
    }

    [RelayCommand]
    private void Delete(string playerAlias)
    {
        _playerControler.RemovePlayer(playerAlias);

        var foundPlayer = Players.FirstOrDefault(p => p.Alias == playerAlias);
        if (foundPlayer != null)
            Players.Remove(foundPlayer);
    }
}
