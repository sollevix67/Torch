﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using NLog;
using Torch.Managers;
using Torch.Session;
using Torch.UI.ViewModels;
using VRage.Game.ModAPI;

namespace Torch.UI.Views
{
    /// <summary>
    ///     Interaction logic for PlayerListControl.xaml
    /// </summary>
    public partial class PlayerListControl : UserControl
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private IMultiplayerManagerServer _mpServer;

        private ITorchServer _server;

        public PlayerListControl()
        {
            InitializeComponent();
        }

        private void OnPlayerPromoted(ulong arg1, MyPromoteLevel arg2)
        {
            Dispatcher.InvokeAsync(() => PlayerList.Items.Refresh());
        }

        public void BindServer(ITorchServer server)
        {
            _server = server;
            _server.Initialized += Server_Initialized;
        }

        private void Server_Initialized(ITorchServer obj)
        {
            var sessionManager = _server.Managers.GetManager<ITorchSessionManager>();
            sessionManager.SessionStateChanged += SessionStateChanged;
        }

        private void SessionStateChanged(ITorchSession session, TorchSessionState newState)
        {
            switch (newState)
            {
                case TorchSessionState.Loaded:
                    Dispatcher.InvokeAsync(() => DataContext = _server?.CurrentSession?.Managers.GetManager<MultiplayerManagerDedicated>());
                    _mpServer = _server.CurrentSession.Managers.GetManager<IMultiplayerManagerServer>();
                    _mpServer.PlayerPromoted += OnPlayerPromoted;
                    break;
                case TorchSessionState.Unloading:
                    Dispatcher.InvokeAsync(() => DataContext = null);
                    break;
            }
        }

        private void KickButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerList.SelectedItem == null)
                return;

            var player = (KeyValuePair<ulong, PlayerViewModel>)PlayerList.SelectedItem;
            try
            {
                _server.CurrentSession.Managers.GetManager<IMultiplayerManagerServer>().KickPlayer(player.Key);
            }
            catch (Exception ex)
            {
                _log.Warn(ex);
            }
        }

        private void BanButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerList.SelectedItem == null)
                return;

            var player = (KeyValuePair<ulong, PlayerViewModel>)PlayerList.SelectedItem;
            try
            {
                _server.CurrentSession.Managers.GetManager<IMultiplayerManagerServer>().BanPlayer(player.Key);
            }
            catch (Exception ex)
            {
                _log.Warn(ex);
            }
        }

        private void PromoteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (PlayerList.SelectedItem == null)
                return;

            var player = (KeyValuePair<ulong, PlayerViewModel>)PlayerList.SelectedItem;
            try
            {
                _server.CurrentSession.Managers.GetManager<IMultiplayerManagerServer>().PromoteUser(player.Key);
            }
            catch (Exception ex)
            {
                _log.Warn(ex);
            }
        }

        private void DemoteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (PlayerList.SelectedItem == null)
                return;

            var player = (KeyValuePair<ulong, PlayerViewModel>)PlayerList.SelectedItem;
            try
            {
                _server.CurrentSession.Managers.GetManager<IMultiplayerManagerServer>().DemoteUser(player.Key);
            }
            catch (Exception ex)
            {
                _log.Warn(ex);
            }
        }
    }
}