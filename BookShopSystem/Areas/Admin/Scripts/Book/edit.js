/**
* 编辑
*/

var id = $.getQueryString("id");


var upload = null;
var datepicker = null;

var toc = UE.getEditor('toc-container', {
    toolbars: [
        ['fullscreen', 'source', 'undo', 'redo', 'bold']
    ],
    autoHeightEnabled: true,
    initialFrameHeight: 200,
    autoFloatEnabled: true
});


var content = UE.getEditor('content-container', {
    toolbars: [
        ['fullscreen', 'source', 'undo', 'redo', 'bold']
    ],
    autoHeightEnabled: true,
    initialFrameHeight: 200,
    autoFloatEnabled: true
});


var author = UE.getEditor('author-container', {
    toolbars: [
        ['fullscreen', 'source', 'undo', 'redo', 'bold']
    ],
    autoHeightEnabled: true,
    initialFrameHeight: 200,
    autoFloatEnabled: true
});

var editor = UE.getEditor('editor-container', {
    toolbars: [
        ['fullscreen', 'source', 'undo', 'redo', 'bold']
    ],
    autoHeightEnabled: true,
    initialFrameHeight: 200,
    autoFloatEnabled: true
});



var Book = new Vue({
    el: '#edit-form',
    data: {
        bookModel:
        {

        },
        parentId: "",
        parentCategoryList: [],
        childCategoryList: [],
        publishersList: [],
        professionList: []
    },
    methods: {
        save: function () {
            var $this = this;
            $this.$validator.validateAll().then((result) => {
                if (!result) {
                    alert($this.errors.items[0].msg);
                }
                else {

                    var img = upload.getData();
                    console.log(img);
                    if (img.length <= 0) {
                        alert("请至少上传一张图片！");
                        return false;
                    }


                    var imgList = [];
                    $.each(img, function (i, file) {
                        var item = {};
                        item.FileName = file.name;
                        item.FilePath = file.path;
                        item.MediaType = file.type;
                        imgList.push(item);
                    });

                    if (id != undefined && id != "") {
                        $this.bookModel.Id = id;
                    }

                    $this.bookModel.TOC = toc.getContent();
                    $this.bookModel.ContentDescription = content.getContent();
                    $this.bookModel.AurhorDescription = author.getContent();
                    $this.bookModel.EditorComment = editor.getContent();
                    $this.bookModel.PublishDate = $("#txtPublishDate").val();
                    $.httpPost("/Admin/Book/SaveBook", { book: $this.bookModel, imgList: imgList }).then(function (req) {
                        if (req.Status) {
                            location.href = "/Admin/Book/Index";
                        }
                        else {
                            alert("保存失败！");
                        }

                    });
                }
            });
        }
    },
    watch: {
        parentId: function (newval, oldval) {
            initCategoryList(newval, true);
        }
    }
});

$(function () {
    //初始化上传组建
    upload = $("#upload").BrilliantWebUploader({
        fileNumLimit: 6,
        accept: {
            title: 'Images',
            extensions: 'jpg,jpeg,png',
            mimeTypes: 'image/*'
        }
    });

    datepicker = $('#txtPublishDate').datepicker({ format: 'yyyy-mm-dd' });;

    initEvert();//初始化事件
    initData();//初始化数据
});


/**
 * 初始化分类
 * @param {number} id 父类编号
 * @param {boolean} isChild 是否是子类
 */
function initCategoryList(id, isChild) {
    $.httpPost("/Admin/Book/GetCategoryList", { parentId: id }).then(function (req) {
        var list = [];
        if (req) {
            list = req;
        }
        if (!isChild) {
            Book.parentCategoryList = list;
        }
        else {
            Book.childCategoryList = list;
        }

    });
}

/**
 * 初始化出版社
 */
function initPublishersList() {
    $.httpPost("/Admin/Book/GetPublishersList").then(function (req) {
        var list = [];
        if (req) {
            Book.publishersList = req;
        }
    });
}

/**
 * 初始化职业
 */
function initProfessionList() {
    $.httpPost("/Admin/Book/GetProfessionList").then(function (req) {
        var list = [];
        if (req) {
            Book.professionList = req;
        }
    });
}

/**
 * 初始化事件
 */
function initEvert() {

    $("#parent-select").on('change', function () {
        Book.parentId = $(this).val();
    });

    $("#child-select").on('change', function () {
        Book.bookModel.CategoryId = $(this).val();
    });

    $("#publisher-select").on('change', function () {
        Book.bookModel.PublisherId = $(this).val();
    });

    $("#profession-select").on('change', function () {
        Book.bookModel.ProfessionId = $(this).val();
    });
    //
}

/**
 * 初始化数据
 */
function initData() {
    initCategoryList(0, false);
    initPublishersList();
    initProfessionList();

    //编辑模式
    if (id != undefined && id != "") {
        $.httpPost("/Admin/Book/GetEditInfo", { productId: id }).then(function (req) {
            if (req.Status) {
                Book.bookModel = req.Data.Book;

              
                if (Book.bookModel.TOC != undefined && Book.bookModel.TOC != "") {
                    toc.ready(function () {
                        toc.setContent(Book.bookModel.TOC);
                    });
                   
                }
                if (Book.bookModel.ContentDescription != undefined && Book.bookModel.ContentDescription != "") {
                    content.ready(function () {
                        content.setContent(Book.bookModel.ContentDescription);
                    });
                   
                }
                if (Book.bookModel.AurhorDescription != undefined && Book.bookModel.AurhorDescription != "") {
                    author.ready(function () {
                        author.setContent(Book.bookModel.AurhorDescription);
                    });
                   
                }
                if (Book.bookModel.EditorComment != undefined && Book.bookModel.EditorComment != "") {
                    editor.ready(function () {
                        editor.setContent(Book.bookModel.EditorComment);
                    });
                }
                datepicker.datepicker('setValue', moment(Book.bookModel.PublishDate).format("YYYY-MM-DD"));
                Book.parentId = Book.bookModel.ParentCategoryId;
                $("#parent-select").val(Book.parentId);
                $("#child-select").val(Book.bookModel.CategoryId);
                $("#publisher-select").val(Book.bookModel.PublisherId);
                $("#profession-select").val(Book.bookModel.ProfessionId);
                $.each(req.Data.ImgList, function (i, item) {
                    var json = {};
                    json.id = item.Id;
                    json.name = item.FileName;
                    json.path = item.FilePath;
                    json.size = 0;
                    json.extension = "";
                    json.md5 = item.Id;
                    json.type = item.MediaType;
                    json.fullPath = "";
                    json.lastModifiedDate = "";
                    upload.setData(json);
                });

            }
            else {
                alert(req.Msg);
            }
        });


    }
}

