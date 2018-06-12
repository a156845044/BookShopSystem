using BookShopSystem.Models;
using BookShopSystem.Service;
using BookShopSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShopSystem.Controllers
{
    /// <summary>
    /// 商品
    /// </summary>
    public class ProductController : FilterController
    {
        // GET: Product/Introduction
        public ActionResult Introduction()
        {
            return View();
        }

        /// <summary>
        /// 获取图片列表
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns>图片列表</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetImgList(long productId)
        {
            var imgList = new UploadService().GetFileList(FlagMgr.Upload.SourceType.Book.ToInt(), productId.ToString());
            return JsonCResult(imgList);
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns>图片列表</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetProductInfo(long productId)
        {
            ReturnEntity<object> data = new ReturnEntity<object>
            {
                Status = false
            };
            BookService service = new BookService();
            var book = service.GetBookDetailInfo(productId);
            if (book != null)
            {
                if (!string.IsNullOrWhiteSpace(book.TOC))
                {
                    book.TOC = Server.HtmlDecode(book.TOC);
                }
                if (!string.IsNullOrWhiteSpace(book.ContentDescription))
                {
                    book.ContentDescription = Server.HtmlDecode(book.ContentDescription);
                }
                if (!string.IsNullOrWhiteSpace(book.EditorComment))
                {
                    book.EditorComment = Server.HtmlDecode(book.EditorComment);
                }
                if (!string.IsNullOrWhiteSpace(book.AurhorDescription))
                {
                    book.AurhorDescription = Server.HtmlDecode(book.AurhorDescription);
                }

                var imgList = new UploadService().GetFileList(FlagMgr.Upload.SourceType.Book.ToInt(), productId.ToString());
                int monthSales = service.GetSaleCountByMonth(productId);
                int sales = service.GetSaleCount(productId);
                int count = service.GetCommentCount(productId);
                data.Status = true;
                var retrunData = new { Product = book, Sales = sales, MonthSales = monthSales, CommentCount = count, ImgList = imgList };
                data.Data = retrunData;
            }
            else
            {
                data.Msg = "未获取到商品详情！";
            }
            return JsonCResult(data);
        }

        /// <summary>
        /// 更新点击量
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult UpdateClick(long productId)
        {
            bool flag = new BookService().UpdateClick(productId);
            return JsonCResult(flag);
        }

        /// <summary>
        /// 获取好书推荐
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetBookRecommendList(long productId)
        {
            BookService service = new BookService();
            List<BookInfoEntity> list = null;
            bool flag = false;
            if (IsLogined)
            {
                var user = LoginUser;
                if (user.ProfessionId > 0)
                {
                    list = service.GetTopNList(user.ProfessionId, 6, productId);
                }
                else
                {
                    flag = true;
                }
            }
            else
            {
                flag = true;
            }
            if (flag)
            {
                list = service.GetTopNList(0, 6, productId);
            }
            return JsonCResult(list);
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetCommentList(CommentRequest page)
        {
            int recordCount = 0;
            var list = new ProductCommentService().GetCommentList(page.ProductId, page.PageIndex, page.PageSize, out recordCount);
            int count = SysHelper.GetPageCount(recordCount, page.PageSize);
            var data = new PageReturnEntity<List<CommentInfoEntity>> { PageCount = count, Data = list };
            return JsonCResult(data);
        }

        
    }
}