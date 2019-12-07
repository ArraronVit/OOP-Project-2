using Foundation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CoreGraphics;
using CoreLocation;
using MapKit;
using ObjCRuntime;
using Realms;
using UIKit;



namespace CProjectMapIos
{
    public partial class ViewController : UIViewController
    {
        MKMapView map;
        MapDelegate mapDelegate;
        private bool annotationMode;
        private List<string> usersTosend = new List<string>();
        private List<MKPointAnnotation> annotations = new List<MKPointAnnotation>();
        public ViewController(IntPtr handle) : base(handle)
        {
        }
        
        UIButton modeButton = new UIButton(UIButtonType.ContactAdd)
        {
            Frame = new CGRect(330.00, 632.00, 35.00, 25.00), 
            BackgroundColor = UIColor.DarkGray,
            TintColor = UIColor.Red
        };
        
        UIButton validateLocationButton = new UIButton(UIButtonType.RoundedRect)
        {
            Frame = new CGRect(0.00, 55.00, 80.00, 40.00),
            BackgroundColor = UIColor.DarkGray,
            TintColor = UIColor.White,
        };
        
        UIButton sendEmailButton = new UIButton(UIButtonType.RoundedRect)
        {
            Frame = new CGRect(295.00, 55.00, 80.00, 40.00),
            BackgroundColor = UIColor.DarkGray,
            TintColor = UIColor.White,
        };
        private void modeButtonEventHandler(object sender, EventArgs e)
        {
            if (sender == modeButton)
            {
                annotationMode = !annotationMode;
                if (modeButton.TintColor.Equals(UIColor.Red))
                {
                    modeButton.BackgroundColor = UIColor.Gray;
                    modeButton.TintColor = UIColor.Blue;
                }
                else
                {
                    modeButton.BackgroundColor = UIColor.DarkGray;
                    modeButton.TintColor = UIColor.Red;
                }
            }
        }
        
        private void validateLocationButtonEventHandler(object sender, EventArgs e)
        {
            if (sender == validateLocationButton)
            {
                var affordableDistance = checkLocation();
                if (affordableDistance != null)
                {
                    validateLocationButton.BackgroundColor = UIColor.Green;
                    validateLocationButton.TintColor = UIColor.DarkGray;
                    sendEmailButton.Enabled = false;
                }
                else
                {
                    validateLocationButton.BackgroundColor = UIColor.Red;
                    sendEmailButton.Enabled = true;
                 
                }
            }
        }

        private void sendEmailButtonEventHandler(object sender, EventArgs e)
        {
            if (sender == sendEmailButton)
            {
                var test = new EmailSender();
                test.SendEmail("Test", "foobar", usersTosend);
            }
        }

        private double? checkLocation()
        {
            var overlays = map.Overlays;
            var user = map.UserLocation;
            var userLatitude = user.Location.Coordinate.Latitude;
            var userLongitude = user.Location.Coordinate.Longitude;
            var affordableDistance = 0.00;
            
            foreach (var overlay in overlays)
            {
                var latitude = overlay.Coordinate.Latitude;
                var longitude = overlay.Coordinate.Longitude;
                var overlapCenterCoordinate = new CLLocation(latitude,longitude);
                var userLocationCoordinate = new CLLocation(userLatitude,userLongitude);
                var distanceFromCenterToUser = overlapCenterCoordinate.DistanceFrom(userLocationCoordinate);
                
                if(distanceFromCenterToUser <= 1000.00)
                {
                    affordableDistance = distanceFromCenterToUser;
                }
            }

            if (affordableDistance != 0.00)
            {
                return affordableDistance;
            }
            else
            {
                return null;
            }
            
        }
        
        public override void ViewDidLoad()
        {
            showUserMap();
            showSearchBar();
            annotationMode = true;
            //var realm = Realm.GetInstance();
            map.AddSubview(modeButton);
            map.AddSubview(validateLocationButton);
            map.AddSubview(sendEmailButton);
            sendEmailButton.Enabled = false;
            
            sendEmailButton.SetTitle("Inform",UIControlState.Normal);
            validateLocationButton.SetTitle("Check",UIControlState.Normal);

            modeButton.AddTarget(modeButtonEventHandler, UIControlEvent.TouchUpInside);
            validateLocationButton.AddTarget(validateLocationButtonEventHandler,UIControlEvent.TouchUpInside);
            sendEmailButton.AddTarget(sendEmailButtonEventHandler,UIControlEvent.TouchUpInside);
            usersTosend.Add("deed8@mail.ru");
            usersTosend.Add("vvit.kozlov@icloud.com");
            usersTosend.Add("egor_kuzmin22@mail.ru");
        }
        
        [Export("MapLongTapSelector:")]
        protected void OnMapLongTapped(UIGestureRecognizer sender)
        {
            CLLocationCoordinate2D tappedLocationCoord =  map.ConvertPoint(sender.LocationInView(map), map);
            if (annotationMode)
            {
                foreach (var annotation in annotations)
                {
                   // var annotation = new MKPointAnnotation(tappedLocationCoord, "User annotation", "");
                   if (annotation.Coordinate.Latitude == tappedLocationCoord.Latitude && annotation.Coordinate.Longitude == tappedLocationCoord.Longitude)
                   {
                       map.RemoveAnnotation(annotation);
                       annotations.Remove(annotation);
                   }
                }
            }
        }
        [Export("MapTapSelector:")]
        protected void OnMapTapped(UIGestureRecognizer sender)
        {
            CLLocationCoordinate2D tappedLocationCoord =  map.ConvertPoint(sender.LocationInView(map), map);
            if (annotationMode)
            {
                var annotation = new MKPointAnnotation(tappedLocationCoord, "User annotation", "");
                annotations.Add(annotation);
                map.AddAnnotation(annotation);
            }
            else
            {
                var overlay = MKCircle.Circle(tappedLocationCoord,1000);
                map.AddOverlay (overlay);
            }
        }
        
        public override void LoadView()
        {
            map = new MKMapView (UIScreen.MainScreen.Bounds);
            View = map;
        }
        
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void showUserMap()
        {
            
            map.MapType = MKMapType.HybridFlyover;
            map.ShowsUserLocation = true;
            map.ZoomEnabled = true;
            map.ScrollEnabled = true;
            map.ShowsBuildings = true;
            map.PitchEnabled = true;
            map.ShowsCompass = false;
            double lat = 30.2652233534254;
            double lon = -97.73815460962083;
            CLLocationCoordinate2D mapCenter = new CLLocationCoordinate2D (lat, lon);
            CLLocationCoordinate2D viewPoint = new CLLocationCoordinate2D(lat + 0.0050, lon - 0.0072);
            MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance (mapCenter, 1500, 1500);
            map.CenterCoordinate = mapCenter;
            map.Region = mapRegion;
            var camera = MKMapCamera.CameraLookingAtCenterCoordinate(mapCenter, viewPoint, 500);
            map.Camera = camera;
            mapDelegate = new MapDelegate ();
            map.Delegate = mapDelegate;
            askUserPermissions();
            
            var tapRecogniser = new UITapGestureRecognizer(this, new Selector("MapTapSelector:"));
            var longTapRecogniser = new UILongPressGestureRecognizer(this, new Selector("MapLongTapSelector:"));
            map.AddGestureRecognizer(tapRecogniser);
            map.AddGestureRecognizer(longTapRecogniser);
            var hotelOverlay = MKCircle.Circle(mapCenter,1000);
            map.AddOverlay(hotelOverlay);
            
        }

        private void showSearchBar()
        {
            var searchResultsController = new SearchResultsViewController (map);
//Creates a search controller updater
            var searchUpdater = new SearchResultsUpdator ();
            searchUpdater.UpdateSearchResults += searchResultsController.Search;

//add the search controller
            var searchController = new UISearchController (searchResultsController)
            {
                SearchResultsUpdater = searchUpdater
            };
           
//format the search bar
            searchController.SearchBar.SizeToFit ();
            searchController.SearchBar.SearchBarStyle = UISearchBarStyle.Minimal;
            searchController.SearchBar.Placeholder = "Enter a search query";
            
//the search bar is contained in the navigation bar, so it should be visible
            searchController.HidesNavigationBarDuringPresentation = false;
//Ensure the searchResultsController is presented in the current View Controller
            DefinesPresentationContext = true;

//Set the search bar in the navigation bar
            NavigationItem.TitleView = searchController.SearchBar;
            map.AddSubview(searchController.SearchBar);
            searchController.SearchBar.BackgroundColor = UIColor.DarkGray;
            searchController.SearchBar.TintColor = UIColor.White;
            searchController.SearchBar.BarTintColor = UIColor.White;
        }

        private void askUserPermissions()
        {
            var locationManager = new CLLocationManager();
            locationManager.RequestWhenInUseAuthorization();
        }
    }
}