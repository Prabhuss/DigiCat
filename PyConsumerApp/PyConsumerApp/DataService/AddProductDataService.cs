using Microsoft.WindowsAzure.Storage.Blob;
using PyConsumerApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.DataService
{

    [Preserve(AllMembers = true)]
    [DataContract]
    class AddProductDataService : BaseService
    {
        private static AddProductDataService instance;
        public static AddProductDataService Instance => instance ?? (instance = new AddProductDataService());
        public async Task<string> AddProductToDataBase(Product NewProduct)
        {
            try
            {
                var app = App.Current as App;
                Dictionary<string, object> payload = new Dictionary<string, object>();
                payload.Add("merchantbranchid", app.Merchantid);
                payload.Add("phone_number", app.UserPhoneNumber);
                payload.Add("access_key", app.SecurityAccessKey);
                payload.Add("productName", NewProduct.productName);
                payload.Add("productId", NewProduct.ProductIdId);
                payload.Add("barcode", NewProduct.QuantityList);
                payload.Add("productDescription", NewProduct.productDesc);
                payload.Add("category", NewProduct.Category);
                payload.Add("subcategory", NewProduct.SubCategory);
                payload.Add("regularPrice", NewProduct.mrp);
                payload.Add("discount", NewProduct.DiscountPrice);
                payload.Add("salePrice", NewProduct.SellingPrice);
                payload.Add("productquant", NewProduct.TotalQuantity);
                payload.Add("productUOM", NewProduct.UOM);
                payload.Add("gst", NewProduct.GST);
                payload.Add("cess", NewProduct.CESS);
                payload.Add("productBrand", NewProduct.SellerName);
                payload.Add("productImageURL", NewProduct.productPicUrl);
                payload.Add("productAvailable", NewProduct.Availability_Status);
                payload.Add("isvisible", NewProduct.Visibility_Status);
                payload.Add("imgflag", "F");
                payload.Add("isdelete", "No");
                AddCatResponse robject = await this.Post<AddCatResponse>(this.getAuthUrl("addProduct_digicat"), payload, null);
                if (robject != null)
                {
                    return robject.Data.Message;
                }
            }
            catch (Exception e)
            {
                var app = App.Current as App;
                Log.Debug("[Error]", "Probably an issue with secret key " + e.Message);
            }
            return null;
        }
        public async Task<string> UpdateProductDetailsToDataBase(Product NewProduct)
        {
            try
            {
                var app = App.Current as App;
                Dictionary<string, object> payload = new Dictionary<string, object>();
                payload.Add("merchantbranchid", app.Merchantid);
                payload.Add("phone_number", app.UserPhoneNumber);
                payload.Add("access_key", app.SecurityAccessKey);
                payload.Add("productName", NewProduct.productName);
                payload.Add("productId", NewProduct.ProductIdId);
                payload.Add("productDescription", NewProduct.productDesc);
                payload.Add("barcode", NewProduct.QuantityList);
                payload.Add("CitrineProdId", NewProduct.CitrineProdId); 
                payload.Add("category", NewProduct.Category);
                payload.Add("subcategory", NewProduct.SubCategory);
                payload.Add("discount", NewProduct.DiscountPrice);
                payload.Add("regularPrice", NewProduct.mrp);
                payload.Add("gst", NewProduct.GST);
                payload.Add("cess", NewProduct.CESS);
                payload.Add("salePrice", NewProduct.SellingPrice);
                payload.Add("productquant", NewProduct.TotalQuantity);
                payload.Add("productUOM", NewProduct.UOM);
                payload.Add("productBrand", NewProduct.SellerName);
                payload.Add("productImageMobile", NewProduct.productPicUrl);
                payload.Add("productAvailable", NewProduct.Availability_Status);
                payload.Add("productVisible", NewProduct.Visibility_Status);
                payload.Add("ImageLinkFlag", NewProduct.ImageLinkFlag);

                AddCatResponse robject = await this.Post<AddCatResponse>(this.getAuthUrl("updateProductDigicat"), payload, null);
                if (robject.STATUS.ToLower() == "success")
                {
                    return robject.Data.Message;
                }
            }
            catch (Exception e)
            {
                var app = App.Current as App;
                Log.Debug("[Error]", "Probably an issue with secret key " + e.Message);
            }
            return null;
        }
        public async Task<SaSTokenModel> GetSasTokenInfoForProductImage()
        {
            try
            {
                var app = App.Current as App;
                Dictionary<string, object> payload = new Dictionary<string, object>();
                payload.Add("merchantbranchid", app.Merchantid);
                payload.Add("access_key", app.SecurityAccessKey);
                payload.Add("phone_number", app.UserPhoneNumber);
                SaSTokenModel robject = await this.Post<SaSTokenModel>(this.getAuthUrl("getBlobCredentials"), payload, null);
                if (robject != null)
                {
                    return robject;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return null;
            }
        }


        public async Task<string> UploadNewCategory(SubCategory NewCat)
        {
            try
            {
                var app = App.Current as App;
                Dictionary<string, object> payload = new Dictionary<string, object>();
                payload.Add("merchantbranchid", app.Merchantid);
                payload.Add("access_key", app.SecurityAccessKey);
                payload.Add("phone_number", app.UserPhoneNumber);
                payload.Add("categoryName", NewCat.Name);
                payload.Add("categoryId", NewCat.CategoryId);
                payload.Add("productImageMobile", "");
                var robject = await this.Post<AddCatResponse>(this.getAuthUrl("addProductCategoryMobile_digicat"), payload, null);
                if (robject != null)
                {
                    return robject.Data.Message;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return null;
            }
        }


        public async Task<string> UploadProductImage(Stream stream, string STORAGEURL, string BLOBSASTOKEN )  
        {
            try
            {
                var fileName = Guid.NewGuid().ToString();
                string BlobUri = STORAGEURL;
                string PIctureNameAndSasUri = fileName + ".jpeg" + BLOBSASTOKEN;
                string UploadUri = BlobUri + PIctureNameAndSasUri;
                var cloudBlockBlob = new CloudBlockBlob(new Uri(UploadUri));
                await cloudBlockBlob.UploadFromStreamAsync(stream);
                return cloudBlockBlob.Uri.OriginalString;
            }
            catch
            {
                return null;
            }
        }
    }

    public class AddCatRESULT
    {
        [DataMember(Name = "Message")]
        public string Message { get; set; }
    }

    public class AddCatResponse
    {
        [DataMember(Name = "STATUS")]
        public string STATUS { get; set; }
        [DataMember(Name = "data")]
        public AddCatRESULT Data { get; set; }
    }
}
