using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.Models
{
    /// <summary>
    /// Model for pages with product.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class Product : INotifyPropertyChanged
    {
        #region Constructor

        public Product()
        {

        }

        public Product(Product p)
        {
            this.MerchantBranchId = p.MerchantBranchId;
            productName = p.productName;
            ProductIdId = p.ProductIdId;
            QuantityList = p.QuantityList;
            productDesc = p.productDesc;
            Category = p.Category;
            SubCategory = p.SubCategory;
            mrp = p.mrp;
            GST = p.GST;
            CESS = p.CESS;
            SellingPrice = p.SellingPrice;
            SellerName = p.SellerName;
            CitrineProdId = p.CitrineProdId;
            DiscountPrice = p.DiscountPrice;
            TotalQuantity = p.TotalQuantity;
            UOM = p.UOM;
            productPicUrl = p.productPicUrl;
            if(p.Visibility_Status != null)
                if (p.Visibility_Status.ToLower() == "yes")
                    VisibilityStatusBool = true;
            if (p.Availability_Status != null)
                if (p.Availability_Status.ToLower() == "yes")
                AvailabilityStatusBool = true;
            Availability_Status = p.Availability_Status;
            Visibility_Status = p.Visibility_Status;
            ImageLinkFlag = p.ImageLinkFlag;
            IsDelete = p.IsDelete;
        }

        #endregion
        #region Fields

        private bool isFavourite;
        private bool anyDiscount;
        private double? gst;
        private double? cess;
        private double? _mrp;
        private double? _SellingPrice;

        private string previewImage;

        [DataMember(Name = "ImageLinkFlag")]
        public string ImageLinkFlag { get; set; }

        [DataMember(Name = "IsDelete")]
        public string IsDelete { get; set; }

        private List<string> previewImages;

        private int totalQuantity;

        private double actualPrice;

        private double? discountPrice;

        private double? discountPercent;

        private ObservableCollection<Review> reviews = new ObservableCollection<Review>();

        #endregion

        #region Event

        /// <summary>
        /// The declaration of property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the property that holds the product id.
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the property that has been bound with Xamarin.Forms Image, which displays the product image.
        /// </summary>
        [DataMember(Name = "previewimage")]
        public string PreviewImage
        {
            get
            {
                if (string.IsNullOrEmpty(ImageLinkFlag))
                {
                    return App.product_url + this.productPicUrl;
                }
                if (ImageLinkFlag.ToLower() == "f")
                {
                    return this.productPicUrl;
                }
                else
                {
                    return App.product_url + this.productPicUrl;
                }
            }
            set { this.productPicUrl = value; }
            // get { return App.BaseImageUrl + this.previewImage; }
            /*  get { return App.product_url + this.previewImage; }
              set { this.previewImage = value; }
              */

        }



        public bool AnyDiscount
        {
            get
            {
                if (SellingPrice < mrp)
                    return true;
                else
                    return false;
            }
            set { this.anyDiscount = value; }
        }
        /// <summary>
        /// Gets or sets the property that has been bound with SfRotator, which displays the item images.
        /// </summary>
        [DataMember(Name = "previewimages")]
        public List<string> PreviewImages
        {
            get
            {
                this.previewImages[0] = App.product_url + this.productPicUrl;
                return this.previewImages;
            }

            set
            {
                this.previewImages = value;
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a label, which displays the product name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the property that has been bound with a label, which displays the product summary.
        /// </summary>
        [DataMember(Name = "summary")]
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the property that has been bound with a label, which displays the product description.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the property that has been bound with a label, which displays the actual price of the product.
        /// </summary>
        [DataMember(Name = "actualprice")]
        public double? ActualPrice
        {
            get
            {
                //return this.actualPrice;
                return this.SellingPrice;
            }

            set
            {
                //this.actualPrice = value;
                this.SellingPrice = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a label, which displays the discounted price of the product.
        /// </summary>
        public double? DiscountPrice
        {
            get
            {
                return discountPrice = mrp-SellingPrice;
            }

            set
            {
                this.discountPrice = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a label, which displays the discounted percent of the product.
        /// </summary>
        [DataMember(Name = "discountpercent")]
        public double? DiscountPercent
        {
            get
            {
                if (mrp < 1)
                    return -SellingPrice;
                return this.discountPercent = (mrp-SellingPrice)*100/mrp;
            }

            set
            {
                this.discountPercent = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with label, which displays the overall rating of the product.
        /// </summary>
        [DataMember(Name = "overallrating")]
        public double OverallRating { get; set; }

        /// <summary>
        /// Gets or sets the property that has been bound with view, which displays the customer review.
        /// </summary>
        [DataMember(Name = "reviews")]
        public ObservableCollection<Review> Reviews
        {
            get
            {
                return this.reviews;
            }

            set
            {
                this.reviews = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with label, which displays the seller.
        /// </summary>
        public string SellerName { get; set; }

        /// <summary>
        /// Gets or sets the property has been bound with SfCombobox which displays selected quantity of product.
        /// </summary>
        [DataMember(Name = "quantities")]
        public List<object> Quantities { get; set; } = new List<object> { 1, 2, 3, 4, 5, 6, 7, 8 ,9  };

        /// <summary>
        /// Gets or sets the property that has been bound with SfCombobox, which displays the product variants.
        /// </summary>
        [DataMember(Name = "sizevariants")]
        public List<string> SizeVariants { get; set; } = new List<string> { "XS", "S", "M", "L", "XL" };

        /// <summary>
        /// Gets or sets a value indicating whether the cart is favorite.
        /// </summary>
        [DataMember(Name = "isfavourite")]
        public bool IsFavourite
        {
            get
            {
                return this.isFavourite;
            }

            set
            {
                this.isFavourite = value;
                this.NotifyPropertyChanged(nameof(IsFavourite));
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with SfCombobox, which displays the total quantity.
        /// </summary>
        [DataMember(Name = "totalquantity")]
        public int TotalQuantity
        {
            get
            {
                return this.totalQuantity;
            }

            set
            {
                this.totalQuantity = value;
                this.NotifyPropertyChanged(nameof(TotalQuantity));
            }
        }


        [DataMember(Name = "cess")]
        public double? CESS
        {
            get
            {
                return this.cess;
            }

            set
            {
                this.cess = value;
                this.NotifyPropertyChanged(nameof(TotalQuantity));
            }
        }

        [DataMember(Name = "GST")]
        public double? GST
        {
            get
            {
                return this.gst;
            }

            set
            {
                this.gst = value;
                this.NotifyPropertyChanged(nameof(TotalQuantity));
            }
        }
        [DataMember(Name = "CitrineProdId")]
        public int CitrineProdId { get; set; }

        [DataMember(Name = "ProductIdId")]
        public string ProductIdId { get; set; }

        [DataMember(Name = "productName")]
        public string productName { get; set; }

        [DataMember(Name = "productDesc")]
        public string productDesc { get; set; }

        [DataMember(Name = "Category")]
        public string Category { get; set; }

        [DataMember(Name = "SubCategory")]
        public string SubCategory { get; set; }

        [DataMember(Name = "mrp")]
        public double? mrp 
        {
            get
            {
                return this._mrp;
            }

            set
            {
                this._mrp = value;
                this.NotifyPropertyChanged(nameof(DiscountPercent));
                this.NotifyPropertyChanged(nameof(DiscountPrice));
            }
        }

        [DataMember(Name = "discount")]
        public double? discount { get; set; }

        [DataMember(Name = "SellingPrice")]
        public double? SellingPrice
        {
            get
            {
                return this._SellingPrice;
            }

            set
            {
                this._SellingPrice = value;
                this.NotifyPropertyChanged(nameof(DiscountPercent));
                this.NotifyPropertyChanged(nameof(DiscountPrice));
            }
        }

        [DataMember(Name = "UOM")]
        public string UOM { get; set; }

        [DataMember(Name = "CreatedBY")]
        public object CreatedBY { get; set; }

        [DataMember(Name = "VisibilityStatus")]
        public string Visibility_Status { get; set; }

        [DataMember(Name = "QuantityList")]
        public string QuantityList { get; set; }

        [DataMember(Name = "CreeatedDate")]
        public DateTime CreeatedDate { get; set; }

        [DataMember(Name = "ModifiedDate")]
        public object ModifiedDate { get; set; }

        [DataMember(Name = "productPicUrl")]
        public string productPicUrl { get; set; }

        [DataMember(Name = "MerchantBranchId")]
        public int MerchantBranchId { get; set; }

        [DataMember(Name = "Availability_Status")]
        public string Availability_Status { get; set; }
        public bool AvailabilityStatusBool { get; set; }
        public bool VisibilityStatusBool { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The PropertyChanged event occurs when changing the value of property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
       
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CopyProductFrom(Product p)
        {
            this.MerchantBranchId = p.MerchantBranchId;
            productName = p.productName;
            ProductIdId = p.ProductIdId;
            QuantityList = p.QuantityList;
            productDesc = p.productDesc;
            Category = p.Category;
            SubCategory = p.SubCategory;
            mrp = p.mrp;
            GST = p.GST;
            CESS = p.CESS;
            CitrineProdId = p.CitrineProdId;
            SellerName = p.SellerName;
            SellingPrice = p.SellingPrice;
            DiscountPrice = p.DiscountPrice;
            TotalQuantity = p.TotalQuantity;
            UOM = p.UOM;
            productPicUrl = p.productPicUrl;
            if (p.Visibility_Status != null)
                if (p.Visibility_Status.ToLower() == "yes")
                    VisibilityStatusBool = true;
            if (p.Availability_Status != null)
                if (p.Availability_Status.ToLower() == "yes")
                    AvailabilityStatusBool = true;
            Availability_Status = p.Availability_Status;
            Visibility_Status = p.Visibility_Status;
            ImageLinkFlag = p.ImageLinkFlag;
            IsDelete = p.IsDelete;
        }
        #endregion
    }
}