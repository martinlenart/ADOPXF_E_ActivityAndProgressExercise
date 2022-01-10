using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityAndProgress.Views
{
    public class MainPageFlyoutMenuItem<TParam>
    {
        public MainPageFlyoutMenuItem()
        {
            TargetType = typeof(MainPageFlyoutMenuItem<TParam>);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
        public TParam Param { get; set; } = default;
    }
}