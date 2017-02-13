using Microsoft.HandsFree.MVVM;
using Microsoft.HandsFree.Settings.Serialization;

namespace Microsoft.HandsFree.Filters
{
    public class Settings : NotifyingObject
    {
        FilterType _activeFilter = FilterType.OneEuroFilter;
        [SettingDescription("Active Filter")]
        public FilterType ActiveFilter
        {
            get { return _activeFilter; }
            set { SetProperty(ref _activeFilter, value); }
        }

        #region Gain Filter settings
        double _saccadeDistance = 0.07;
        [SettingDescription("Saccade Distance", 0.0, 1.0, 0.01)]
        public double SaccadeDistance
        {
            get { return _saccadeDistance; }
            set { SetProperty(ref _saccadeDistance, value); }
        }

        double _gain = 0.04;
        [SettingDescription("Gain (default 0.04)", 0.0, 1.0, 0.01)]
        public double Gain
        {
            get { return _gain; }
            set { SetProperty(ref _gain, value); }
        }

        #endregion

        #region Stampe Filter settings

        int _historyLength = 15;
        [SettingDescription("History Length (frames)", 0, 30, 1)]
        public int HistoryLength
        {
            get { return _historyLength; }
            set { SetProperty(ref _historyLength, value); }
        }

        #endregion

        #region OneEuro Filter settings

        double _beta = 5;
        [SettingDescription("Beta", 1, 10, 0.5)]
        public double Beta
        {
            get { return _beta; }
            set { SetProperty(ref _beta, value); }
        }

        double _cutoff = 0.1;
        [SettingDescription("Cutoff (default 0.1)", 0.01, 2, 0.01)]
        public double Cutoff
        {
            get { return _cutoff; }
            set { SetProperty(ref _cutoff, value); }
        }

        #endregion
    }

}
