
/**
* 图书管理
*/


var Book = new Vue({
    el: '#app-book',
    data: {
        key: ""
    },
    methods: {
        add: function () {
            window.location.href = "/Admin/Book/Edit";
        },
        search: function () {
            initTable();//初始化表格
        }

    }
});

$(function () {
    initTable();//初始化表格
});

/**
 *初始化表格
 */
function initTable() {
    $('#table-list').dataTable({
        "sort": false,
        "autoWidth": false,
        "destroy": true,
        "processing": false,
        "serverSide": false,
        "searching": false,
        "bLengthChange": false,
        "ajax": {
            "url": "/Admin/Book/GetBookList",
            "type": "POST",
            "data": function (obj) {
                obj.__RequestVerificationToken = $("input[name='__RequestVerificationToken']").val();
                obj.key = Book.key;
            },
            "sortable": false
        },
        "columns": [
            {
                "data": null,
                createdCell: function (nTd, sData, oData, iRow) {
                    var startnum = this.api().page() * (this.api().page.info().length);
                    $(nTd).html(iRow + 1 + startnum);
                },
                "sortable": false
            },
            { "data": "CoverURL" },
            { "data": "Title" },
            { "data": "ISBN", "className": "text-center" },
            { "data": "Author", "className": "text-center" },
            { "data": "CategoryId" },
            { "data": "Id" }
        ],
        "columnDefs": [

            {
                "targets": [1],
                "data": "CoverURL",
                "render": function (data, type, full) {
                    return "  <img src=\"" + data + "\" class=\"tpl-table-line-img\"  onclick=\"previewPhoto(" + full.Id + ",'" + full.Title + "','" + full.CoverURL + "')\" style=\"height:40px;cursor: pointer;\" alt=\"\">";
                }
            }, {
                "targets": [5],
                "data": "CategoryId",
                "render": function (data, type, full) {
                    if (full) {
                        return full.ParentCategoryName + "/" + full.CategoryName;
                    }
                }
            },
            {
                "targets": [6],
                "data": "Id",
                "render": function (data, type, full) {
                    var str = "";
                    str += "<a onclick=\"edit(" + data + ")\" href=\"javascript:void (0);\" class=\"btn-link\">修改</a>  ";
                    str += "<a onclick=\"deleteBook(" + data + ")\" href=\"javascript:void(0);\" class=\"btn-link\">删除</a>  ";
                    return str;
                }
            }
        ]
    });
}

/**
 * 预览相册
 * @param {any} id 主键
 * @param {any} name 图书名称
 * @param {any} path 封面路径
 */
function previewPhoto(id, name, path) {

    var json = {
        "title": name, //相册标题
        "id": id, //相册id
        "start": 0, //初始显示的图片序号，默认0
        "data": []//
    };

    var img = {
        "alt": name,
        "pid": id, //图片id
        "src": path, //原图地址
        "thumb": "" //缩略图地址
    };

    var imgList = [];

    $.httpPost("/Admin/Book/GetImgList", { productId: id }).then(function (req) {
        if (req) {
            $.each(req, function (i, item) {
                var imgItem = {};
                imgItem.alt = item.FileName;
                imgItem.pid = item.Id;
                imgItem.src = item.FilePath;
                imgItem.thumb = "";
                imgList.push(imgItem);
            });
        }
        else {
            imgList.push(img);
        }
        json.data = imgList;
        layer.photos({
            photos: json //格式见API文档手册页
            , anim: 5 //0-6的选择，指定弹出图片动画类型，默认随机
        });
    });
}

/**
 * 修改
 * @param {any} id
 */
function edit(id) {
    window.location.href = "/Admin/Book/Edit?id=" + id;
}

/**
 * 删除图书
 * @param {any} id 待删除的图书编号
 */
function deleteBook(id){
    layer.confirm("您确定要删除吗？", { icon: 3, title: '提示' }, function (index) {
        $.httpPost("/Admin/Book/Delete", { productId: id}).then(function (req) {
            if (req) {
                alert("删除成功！");
                initTable();//初始化表格
            }
            else {
                alert("啊哦！删除操作失败了~");
            }
            layer.close(index);
        });
    });
}



