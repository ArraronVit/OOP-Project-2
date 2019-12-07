using System;
using CoreGraphics;
using CoreLocation;
using Foundation;
using MapKit;
using UIKit;

namespace CProjectMapIos
{
    public class MapDelegate: MKMapViewDelegate
    {
        static string annotationId = "UserAnnotation";
        UIImageView venueView;
        //UIImage venueImage;
        
        public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, IMKAnnotation mkAnnotation)
        {
            MKAnnotationView annotationView = null;

            if (mkAnnotation is MKUserLocation)
                return null;

            if (mkAnnotation is UserAnnotation)
            {

                // show conference annotation
                annotationView = mapView.DequeueReusableAnnotation (annotationId);

                if (annotationView == null)
                    annotationView = new MKAnnotationView (mkAnnotation, annotationId);

               // annotationView.Image = UIImage.FromFile ("images/conference.png");
                annotationView.CanShowCallout = true;
                annotationView.Draggable = true;
            }

            return annotationView;
        }

        public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView view)
        {
            if (view.Annotation is UserAnnotation) 
            {

                venueView = new UIImageView ();
                venueView.ContentMode = UIViewContentMode.ScaleAspectFit;
                //venueImage = UIImage.FromFile ("image/venue.png");
               // venueView.Image = venueImage;
                view.AddSubview (venueView);

                UIView.Animate (0.4, () => {
                    venueView.Frame = new CGRect (-75, -75, 200, 200); });
            }
        }

        public override void DidDeselectAnnotationView(MKMapView mapView, MKAnnotationView view)
        {
            if (view.Annotation is UserAnnotation) 
            {
                venueView.RemoveFromSuperview ();
                venueView.Dispose ();
                venueView = null;
            }
        }
        
        public override MKOverlayView GetViewForOverlay(MKMapView mapView, IMKOverlay overlay)
        {
//            var polygon = overlay as MKPolygon;
//            var polygonView = new MKPolygonView (polygon);
//            polygonView.FillColor = UIColor.Blue;
//            polygonView.StrokeColor = UIColor.Red;
//            return polygonView;
            var circleOverlay = overlay as MKCircle;
            var circleView = new MKCircleView (circleOverlay);
            circleView.FillColor = UIColor.Blue;
            circleView.Alpha = (nfloat) 0.1;
            return circleView;
        }
       
    }
}