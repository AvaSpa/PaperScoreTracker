using Application.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace PaperScoreTracker.ViewModels;

public partial class PlayerListViewModel : ObservableObject
{
    [ObservableProperty]
    private string _gameName;

    [ObservableProperty]
    private bool _reverseScoring;

    [ObservableProperty]
    private ObservableCollection<PlayerDecoratorViewModel> _players;

    protected readonly GameControler _gameControler;
    private readonly IPopupService _popupService;

    private bool _orderPlayers;

    public PlayerListViewModel(GameControler gameControler, IPopupService popupService, bool orderPlayers = false)
    {
        _gameControler = gameControler;
        _popupService = popupService;
        _orderPlayers = orderPlayers;

        _players = new ObservableCollection<PlayerDecoratorViewModel>();
    }

    [RelayCommand]
    protected async Task Appearing()
    {
        await LoadGameData();
    }

    protected async Task LoadGameData()
    {
        GameName = await _gameControler.GetGameName();
        ReverseScoring = await _gameControler.GetReverseScoring();
        await UpdatePlayers();
    }

    internal async Task ReloadPlayers()
    {
        await UpdatePlayers();
    }

    private async Task UpdatePlayers()
    {
        Players.Clear();

        var players = await _gameControler.GetAllPlayers(_orderPlayers);
        foreach (var player in players)
        {
            Players.Add(new PlayerDecoratorViewModel(_gameControler, _popupService, player, this));
        }
    }
}
