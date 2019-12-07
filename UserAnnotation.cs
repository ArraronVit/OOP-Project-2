using CoreLocation;
using MapKit;
using Realms;

namespace CProjectMapIos
{
    public class UserAnnotation : MKAnnotation
    {
        public UserAnnotation (string title, CLLocationCoordinate2D coordinate)
        {
            this.Title = title;
            this.Coordinate = coordinate;
        }

        public override string Title { get; }

        public override CLLocationCoordinate2D Coordinate { get; }
    }
}