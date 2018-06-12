using BookShopSystem.Controllers;
using BookShopSystem.DataAccess.Context;
using BookShopSystem.DataAccess.Entity;
using BookShopSystem.Models;
using BookShopSystem.Service;
using BookShopSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShopSystem.Areas.Admin.Controllers
{
    public class BookController : BaseController
    {
        // GET: Admin/Book
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 获取图书列表
        /// </summary>
        /// <param name="pModel"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBookList(DataTableEntity pModel, string key)
        {
            int recordCount = 0;
            var list = new BookService().GetBookList(key, pModel.PageIndex, pModel.PageSize, out recordCount);
            var data = new DataTableReturnEntity<BookInfoEntity>
            {
                data = list,
                draw = pModel.Draw,
                recordsFiltered = recordCount,
                recordsTotal = recordCount
            };
            return JsonCResult(data);
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="parentId">分类列表</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetCategoryList(long parentId)
        {
            var list = new CategoryService().GetCategoryList(parentId);
            return JsonCResult(list);
        }

        /// <summary>
        /// 获取出版社列表
        /// </summary>
        /// <returns>出版社列表</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetPublishersList()
        {
            return JsonCResult(new PublishersService().GetPublisherList());
        }

        /// <summary>
        /// 获取出版社列表
        /// </summary>
        /// <returns>出版社列表</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetProfessionList()
        {
            return JsonCResult(new ProfessionService().GetList());
        }

        /// <summary>
        /// 添加图书
        /// </summary>
        /// <param name="book">待添加的图书</param>
        /// <param name="imgList">待添加的图片</param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveBook(Product book, List<FileUpload> imgList)
        {
            ReturnEntity<string> data = new ReturnEntity<string>
            {
                Status = false
            };

            if (!string.IsNullOrWhiteSpace(book.TOC))
            {
                book.TOC = Server.HtmlEncode(book.TOC);
            }
            if (!string.IsNullOrWhiteSpace(book.ContentDescription))
            {
                book.ContentDescription = Server.HtmlEncode(book.ContentDescription);
            }
            if (!string.IsNullOrWhiteSpace(book.EditorComment))
            {
                book.EditorComment = Server.HtmlEncode(book.EditorComment);
            }
            if (!string.IsNullOrWhiteSpace(book.AurhorDescription))
            {
                book.AurhorDescription = Server.HtmlEncode(book.AurhorDescription);
            }

            var service = new BookService();

            if (book.Id <= 0)//添加
            {
                data.Status = service.AddBook(book, imgList);
            }
            else//编辑
            {
                data.Status = service.UpdateBook(book, imgList);
            }
            return JsonCResult(data);
        }

        /// <summary>
        /// 获取图片列表
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns>图片列表</returns>
        [HttpPost]
        public ActionResult GetImgList(long productId)
        {
            var imgList = new UploadService().GetFileList(FlagMgr.Upload.SourceType.Book.ToInt(), productId.ToString());
            return JsonCResult(imgList);
        }

        /// <summary>
        /// 获取待编辑内容信息
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns>内容信息</returns>
        [HttpPost]
        public ActionResult GetEditInfo(long productId)
        {
            var book = new BookService().GetBookInfo(productId);
            ReturnEntity<object> data = new ReturnEntity<object>
            {
                Status = false
            };
            if (book != null)
            {
                var imgList = new UploadService().GetFileList(FlagMgr.Upload.SourceType.Book.ToInt(), productId.ToString());
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

                var returnData = new {
                    Book= book,
                    ImgList= imgList
                };

                data.Status = true;
                data.Data = returnData;
            }

            data.Msg = "未获取到商品信息";
            return JsonCResult(data);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns>执行结果</returns>
        [HttpPost]
        public ActionResult Delete(long productId)
        {
            bool flag = new BookService().DeleteBook(productId);
            return JsonCResult(flag);
        }
    }
}