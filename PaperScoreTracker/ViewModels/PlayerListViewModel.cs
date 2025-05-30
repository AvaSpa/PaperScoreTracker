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

    public PlayerListViewModel(GameControler gameControler, IPopupService popupService)
    {
        _gameControler = gameControler;
        _popupService = popupService;

        _players = new ObservableCollection<PlayerDecoratorViewModel>();
    }

    [RelayCommand]
    protected async Task Appearing()
    {
        await LoadGameData();
    }

    public async Task DeletePlayer(string playerAlias)
    {
        await _gameControler.RemovePlayer(playerAlias);

        var foundPlayer = Players.FirstOrDefault(p => p.PlayerAlias == playerAlias);
        if (foundPlayer != null)
            Players.Remove(foundPlayer);
    }

    protected async Task ReloadPlayers()
    {
        await UpdatePlayers(true);
    }

    private async Task LoadGameData()
    {
        GameName = await _gameControler.GetGameName();
        ReverseScoring = await _gameControler.GetReverseScoring();
        await UpdatePlayers(false);
    }

    private async Task UpdatePlayers(bool ordered)
    {
        Players.Clear();

        var players = await _gameControler.GetAllPlayers(ordered);
        foreach (var player in players)
        {
            Players.Add(new PlayerDecoratorViewModel(_gameControler, _popupService, player, this));
        }
    }
}
