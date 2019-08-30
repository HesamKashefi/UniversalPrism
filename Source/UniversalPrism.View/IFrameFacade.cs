﻿using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
using UniversalPrism.Core.Navigation;

namespace UniversalPrism.View
{
    public interface IFrameFacade
    {
        bool CanGoBack();
        event EventHandler CanGoBackChanged;
        Task<INavigationResult> GoBackAsync(INavigationParameters parameters, NavigationTransitionInfo infoOverride);

        bool CanGoForward();
        event EventHandler CanGoForwardChanged;
        Task<INavigationResult> GoForwardAsync(INavigationParameters parameters);

        Task<INavigationResult> RefreshAsync();

        Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameter, NavigationTransitionInfo infoOverride);

        INavigationParameters CurrentParameters { get; }
    }
}
