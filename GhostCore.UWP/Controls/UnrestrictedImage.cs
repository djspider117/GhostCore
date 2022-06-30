using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using GhostCore;
using Windows.UI.Xaml.Media.Imaging;
using GhostCore.UWP.Utils;
using System.Reflection;
using GhostCore.Logging;

namespace Bedrock.SDK.UI.Controls.Media.Imaging
{
    public class UnrestrictedImage : Control
    {
        #region Dependency Properties

        public static readonly DependencyProperty DecodePixelWidthProperty =
            DependencyProperty.Register(nameof(DecodePixelWidth), typeof(int), typeof(UnrestrictedImage), new PropertyMetadata(0));

        public static readonly DependencyProperty DecodePixelHeightProperty =
            DependencyProperty.Register(nameof(DecodePixelHeight), typeof(int), typeof(UnrestrictedImage), new PropertyMetadata(0));

        public static readonly DependencyProperty UriSourceProperty =
            DependencyProperty.Register(nameof(UriSource), typeof(Uri), typeof(UnrestrictedImage), new PropertyMetadata(null, (d, e) => (d as UnrestrictedImage).OnUriSourceChanged(e)));

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(UnrestrictedImage), new PropertyMetadata(Stretch.Uniform));

        // Using a DependencyProperty as the backing store for DisableCache.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisableCacheProperty =
            DependencyProperty.Register(nameof(DisableCache), typeof(bool), typeof(UnrestrictedImage), new PropertyMetadata(false));


        #endregion

        #region Events

        public event ExceptionRoutedEventHandler ImageFailed;
        public event RoutedEventHandler ImageOpened;

        #endregion

        #region Fields

        protected Image _imageControl;
        protected BitmapImage _bitmapImage;

        #endregion

        #region Properties

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        public Uri UriSource
        {
            get { return (Uri)GetValue(UriSourceProperty); }
            set { SetValue(UriSourceProperty, value); }
        }
        public int DecodePixelWidth
        {
            get { return (int)GetValue(DecodePixelWidthProperty); }
            set { SetValue(DecodePixelWidthProperty, value); }
        }
        public int DecodePixelHeight
        {
            get { return (int)GetValue(DecodePixelHeightProperty); }
            set { SetValue(DecodePixelHeightProperty, value); }
        }

        public bool DisableCache
        {
            get => (bool)GetValue(DisableCacheProperty);
            set => SetValue(DisableCacheProperty, value);
        }


        #endregion

        #region Constructors and Initialization

        public UnrestrictedImage()
        {
            _bitmapImage = new BitmapImage();
            DefaultStyleKey = typeof(UnrestrictedImage);
            Unloaded += UnrestrictedImage_Unloaded;
        }

        protected override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _imageControl = GetTemplateChild("PART_Image") as Image;

            _imageControl.ImageOpened += ImageControl_ImageOpened;
            _imageControl.ImageFailed += ImageControl_ImageFailed;
            _imageControl.Loaded += ImageControl_ImageOpened;
            _imageControl.Source = _bitmapImage;

            await SetImageSource(UriSource);
        }

        #endregion

        #region Cleanup

        private void UnrestrictedImage_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= UnrestrictedImage_Unloaded;
            if (_imageControl != null)
            {
                _imageControl.ImageOpened -= ImageControl_ImageOpened;
                _imageControl.ImageFailed -= ImageControl_ImageFailed;
                _imageControl.Loaded -= ImageControl_ImageOpened;
                _imageControl.Source = null;
            }
        }

        #endregion

        #region Event Handlers

        private void ImageControl_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ImageFailed?.Invoke(sender, e); // this is here so the event triggers even if the user doesn't call base.OnImageFailed
            OnImageFailed(sender, e);
        }
        private void ImageControl_ImageOpened(object sender, RoutedEventArgs e)
        {
            ImageOpened?.Invoke(sender, e); // this is here so the event triggers even if the user doesn't call base.OnImageOpened
            OnImageOpened(sender, e);
        }

        protected virtual void OnImageFailed(object sender, ExceptionRoutedEventArgs e) { }
        protected virtual void OnImageOpened(object sender, RoutedEventArgs e) { }

        #endregion

        private async void OnUriSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            var uri = e.NewValue as Uri;
            await SetImageSource(uri);
        }

        protected virtual async Task SetImageSource(Uri uri)
        {
            if (_imageControl == null)
                return;

            if (uri == null)
            {
                _bitmapImage.UriSource = null;
                return;
            }

            if (uri.Scheme == "https" || uri.Scheme == "http")
            {
                _bitmapImage.UriSource = uri;
                return;
            }

            if (uri.Scheme == "file")
            {
                var resolvedUri = ResolveUri(uri);

                IRandomAccessStream stream = null;
                var file = await StorageFile.GetFileFromPathAsync(resolvedUri.LocalPath);
                stream = await file.OpenReadAsync();

                try
                {
                    stream.Seek(0);
                    await _bitmapImage.SetSourceAsync(stream);

                    //_bitmapImage.DecodePixelWidth = Math.Min((int)Window.Current.Bounds.Width, _bitmapImage.PixelWidth);

                    UpdateDecodeValues();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.ToString());
                }
            }
        }

        #region Helpers

        protected virtual Uri ResolveUri(Uri uri)
        {
            //if (uri.Scheme == "x-content")
            //{
            //    var globalSettings = ServiceLocator.Instance.Resolve<Foundation.Data.App.AppContext>();
            //    var localPath = $"{globalSettings.RootFolder}{uri.LocalPath.Replace('/', '\\')}";
            //    uri = new Uri(localPath);
            //}

            return uri;
        }

        protected virtual void UpdateDecodeValues()
        {
            _bitmapImage.DecodePixelWidth = DecodePixelWidth;
            _bitmapImage.DecodePixelHeight = DecodePixelHeight;
        }

        #endregion

    }
}
