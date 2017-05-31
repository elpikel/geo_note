﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using TagLife;
using TagLife.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using System.Diagnostics;
using System.IO;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Java.IO;
using Console = System.Console;
using File = Java.IO.File;

[assembly: ExportRenderer(typeof(CustomMap), typeof(ExtendedMapRenderer))]
namespace TagLife.Droid.Renderers
{
    public class ExtendedMapRenderer : MapRenderer, GoogleMap.IOnMarkerClickListener, IOnMapReadyCallback
    {
        GoogleMap map;
        List<CustomPin> customPins;
        bool isDrawn;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                formsMap.PropertyChanged += FormsMap_PropertyChanged;
                //                customPins = formsMap.CustomPins;
                ((MapView)Control).GetMapAsync(this);
            }
        }

        private void FormsMap_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.PropertyName);
        }


        //public new void OnCameraChange(CameraPosition position)
        //{
        //    map.Clear();

        //    foreach (var pin in customPins)
        //    {
        //        var marker = new MarkerOptions();
        //        marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
        //        marker.SetTitle(pin.Pin.Label);
        //        marker.SetSnippet(pin.Pin.Address);
        //        //                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));

        //        map.AddMarker(marker);
        //    }
        //}

//        public new void OnMapReady(GoogleMap googleMap)
//        {
////           
//
//            map = googleMap;
//            map.SetOnMarkerClickListener(this);
//            map.MyLocationEnabled = true;
//            //                    map.SetOnCameraChangeListener(this);
//        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Console.WriteLine(e.PropertyName);
            map = NativeMap;

            if (map != null)
            {
                map.SetOnMarkerClickListener(this);
            }

            if ((e.PropertyName.Equals("VisibleRegion") || e.PropertyName.Contains("Pin")) && !isDrawn && map != null)
            {
                map.Clear();

                foreach (var pin in (sender as CustomMap).CustomPins)
                {
                    var marker = new MarkerOptions();
                    marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
                    marker.SetTitle(pin.Pin.Label);
                    marker.SetSnippet(pin.Pin.Address);

                    var inflater = Android.App.Application.Context.GetSystemService("layout_inflater") as Android.Views.LayoutInflater;


                    var inflate = inflater.Inflate(Resource.Layout.Pinlayout, null);
                
                    

                    var findViewById = inflate.FindViewById<TextView>(Resource.Id.jols);
                    findViewById.Text = "nowyyy";

                    inflate.Measure(MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified), MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));

                    inflate.Layout(0, 0, inflate.MeasuredWidth, inflate.MeasuredHeight);


                    inflate.DrawingCacheEnabled = true;
                    inflate.BuildDrawingCache(true);
                    var drawingCache = inflate.GetDrawingCache(true);


                    ExportBitmapAsPNG(drawingCache);
                    marker.SetIcon(BitmapDescriptorFactory.FromBitmap(drawingCache));
                  
                    //                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));

                    map.AddMarker(marker);
                }
               // isDrawn = true;
            }
        }

        void ExportBitmapAsPNG(Bitmap bitmap)
        {
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = System.IO.Path.Combine(sdCardPath, "test.png");
            var stream = new FileStream(filePath, FileMode.Create);
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Close();
        }

        public bool OnMarkerClick(Marker marker)
        {
            throw new NotImplementedException();
        }
    }
}