using System;
using UIKit;
using Xamarin.Forms;

namespace CProjectMapIos
{
    public partial class DetailViewController: UIViewController
    {
        private UILabel LargeLabel;
        public override void ViewDidLoad()
        {
            
            NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;

            // to demonstrate layout
            LargeLabel.BackgroundColor = UIColor.LightGray;

            var safeGuide = View.SafeAreaLayoutGuide;
            LargeLabel.TopAnchor.ConstraintEqualTo(safeGuide.TopAnchor).Active = true;
            LargeLabel.BottomAnchor.ConstraintEqualTo(safeGuide.BottomAnchor).Active = true;
            //LargeLabel.LeadingAnchor.ConstraintEqualTo(safeGuide.LeadingAnchor).Active = true;
            //LargeLabel.TrailingAnchor.ConstraintEqualTo(safeGuide.TrailingAnchor).Active = true;


            var margins = View.LayoutMarginsGuide;
            LargeLabel.TopAnchor.ConstraintEqualTo(margins.TopAnchor).Active = true;
            LargeLabel.BottomAnchor.ConstraintEqualTo(margins.BottomAnchor).Active = true;
            //LargeLabel.LeadingAnchor.ConstraintEqualTo(margins.LeadingAnchor).Active = true;
            //LargeLabel.TrailingAnchor.ConstraintEqualTo(margins.TrailingAnchor).Active = true;
        }

        public void SetTitle(string title)
        {
            Title = title;
            LargeLabel.Text = $"The title \"{title}\" is {title.Length} characters long.";
        }

        public DetailViewController(IntPtr handle) : base(handle)
        {
            LargeLabel = new UILabel
            {
                Frame = new CoreGraphics.CGRect(10, 100, View.Bounds.Width - 20, View.Bounds.Height / 2)
            };
        }
    }
}