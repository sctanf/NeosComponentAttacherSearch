using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComponentSelectorSearch
{
    internal class SelectorDetails
    {
        private static readonly Type componentSelectorType = typeof(ComponentSelector);
        private static readonly MethodInfo onAddComponentPressedMethod = componentSelectorType.GetMethod("OnAddComponentPressed", AccessTools.allDeclared);
        private static readonly MethodInfo onCancelPressedMethod = componentSelectorType.GetMethod("OnCancelPressed", AccessTools.allDeclared);
        private static readonly MethodInfo onOpenCategoryPressedMethod = componentSelectorType.GetMethod("OnOpenCategoryPressed", AccessTools.allDeclared);
        private static readonly MethodInfo openGenericTypesPressedMethod = componentSelectorType.GetMethod("OpenGenericTypesPressed", AccessTools.allDeclared);

        public TextEditor Editor => SearchBar.Editor.Target;

        public bool HasSearchBar => SearchBar != null;

        public string LastPath { get; set; }

        public CancellationTokenSource LastResultUpdate { get; set; } = new CancellationTokenSource();

        public string LastSearch { get; set; }

        public ButtonEventHandler<string> OnAddComponentPressed { get; }

        public ButtonEventHandler OnCancelPressed { get; }

        public ButtonEventHandler<string> OnOpenCategoryPressed { get; }

        public ButtonEventHandler<string> OpenGenericTypesPressed { get; }

        public TextField SearchBar { get; set; }

        public Text Text => (Text)Editor.Text.Target;

        public SelectorDetails(ComponentSelector selector)
        {
            OnAddComponentPressed = AccessTools.MethodDelegate<ButtonEventHandler<string>>(onAddComponentPressedMethod, selector);
            OnOpenCategoryPressed = AccessTools.MethodDelegate<ButtonEventHandler<string>>(onOpenCategoryPressedMethod, selector);
            OpenGenericTypesPressed = AccessTools.MethodDelegate<ButtonEventHandler<string>>(openGenericTypesPressedMethod, selector);
            OnCancelPressed = AccessTools.MethodDelegate<ButtonEventHandler>(onCancelPressedMethod, selector);
        }

        public static SelectorDetails New(ComponentSelector selector) => new(selector);
    }
}