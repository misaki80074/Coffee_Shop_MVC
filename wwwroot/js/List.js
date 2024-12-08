/////////////////////////下方為主要功能//////////////////////////////////
// 定義媒體查詢條件
var mediaQuery = window.matchMedia('(max-width: 576px)');

// 監聽視窗大小變化
mediaQuery.addEventListener('change', handleMediaChange);

//瀏覽器點擊
window.onclick = function (e) {
    closeModal(e)
}

//瀏覽器滾動
window.onscroll = function () {
    scrollFunction();
}

//瀏覽器載入
window.onload = function () {
    var pathname = window.location.pathname
    if (pathname.includes("List")) {
        console.log("這是清單頁面")
        setUI()
        getProData(getAjaxUrl())
        // RWD初次執行檢查
        handleMediaChange(mediaQuery);
    }
    GetUserid()
}

//瀏覽器歷史紀錄切換
window.onpopstate = function () {
    setUI()
    getProData(getAjaxUrl())
}

/////////////////////////下方為主要功能//////////////////////////////////
//導覽列顯示姓名
function GetUserid() {
    $.ajax({
        url: "/Account/GetUserid",
        method: "GET",
        success: function (data) {
            if (data != "0") {
                $('.loginStatus').text(`Hi! ${data.name}`)
            } else {
                return
            }
        }
    })
}


//篩選的關閉按鈕
function filterBtnClose(self) {
    $(self).closest('.dropdown-menu').removeClass('show');
    $(self).closest('.dropdown-menu').prev().removeClass('show');
}

//產品卡顯示產品浮窗
function cardBtnAdd(self) {
    $.ajax({
        url: "/List/ShowProductModal",
        method: "POST",
        data: { proID: `${$(self).data('id')}` },
        success: function (data) {
            $('body').append(data);
        }
    });
}

//產品浮窗的關閉按鈕
function modalBtnClose(self) {
    $(self).closest('.modalFixed').remove()
}

//產品浮窗的Uom數量更新
function modalBtnUom(self) {
    var input = $(self).parent('.modalUOMcomponent').find('input')
    var uom = parseInt($(input).val())
    Update_Btn_Uom(uom, self, input)
}

// X 產品浮窗加入購物籃
function modalBtnAdd(self) {
    var data =
    {
        proID: $(self).data('id'),
        img: $(self).data('img'),
        name: $(self).data('name'),
        price: parseInt($(self).data('price')),
        qty: parseInt($(self).closest('.productFormat').find('input[type="number"]').val()),
    }
    setLsHtml(data)
}

// X 購物籃產品數量加減
function cartBtnUom(self) {
    var input = $(self).parent('.cartBtn').find('input')
    var uom = parseInt($(input).val())
    Update_Btn_Uom(uom, self, input)
    var cartItem = $(self).closest('.cart')
    var data =
    {
        proID: $(cartItem).find('.card-title').data('id'),
        img: $(cartItem).find('img').prop('src'),
        name: $(cartItem).find('.card-title').text(),
        price: parseInt($(cartItem).find('span').text()),
        qty: parseInt($(cartItem).find('input[type="number"]').val()),
    }
    updateLs(data)
}

// X 移除購物籃產品
function cartBtnClose(self) {
    var cartItem = $(self).parent('.cart')
    var id = cartItem.find('.card-title').data('id')
    removeLs(id)
    cartItem.remove()
}

//點擊時滾動頁面
$('.topbtn').on('click', function () {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
})

//分類點擊事件
$('.accordion a').on('click', function () {
    var text = $(this).text().trim()
    $(this).closest('.collapse').removeClass('show')
    $(this).closest('.accordion').removeClass('show')
    $(this).closest('.accordion-item').find('button').addClass('collapsed')
    $(this).closest('.col-12').find('.navbar-toggler').addClass('collapsed')
    if (text == "所有商品" || text == "濾掛系列") {
        history.pushState({ pathname: "column" }, "", window.location.origin + `/List/${text}`)
        getProData(getAjaxUrl())
    } else {
        var col_text = $(this).closest('.accordion-item').find('button').text()
        history.pushState({ pathname: "column" }, "", window.location.origin + `/List/${col_text}/${text}`)
        getProData(getAjaxUrl())
    }
    setBreadcrumb()
})

//價格排序點擊事件
$('.priceSort li').on('click', function () {
    var text = $(this).text()
    $(this).closest('div').find('button').text(text)
    if (text == "綜合") {
        $(this).closest('div').find('button').text("排序")
        deleteUrl("sort")
    } else if (text.includes("由低到高")) {
        setUrl("sort", "desc")
    } else if (text.includes("由高到低")) {
        setUrl("sort", "asc")
    }
})

//每頁顯示點擊事件
$('.itemShow li').on('click', function () {
    //設定問號參數map
    $(this).closest('div').find('button').text($(this).text())
    var queryMap = new URLSearchParams(window.location.search);
    queryMap.delete("page")
    queryMap.set("item", $(this).data('num'))
    history.pushState({ key: "set" }, "", window.location.origin + window.location.pathname + "?" + queryMap)
    getProData(getAjaxUrl())
})

//篩選點擊事件:價格
$('.filter input[type="submit"]').on('click', function () {
    //點選Go按鈕關閉篩選選單
    filterBtnClose(this)
    //設定問號參數map
    var min = $('.filterPriceItem').find('input[name="min"]').val()
    var max = $('.filterPriceItem').find('input[name="max"]').val()
    if (min == "" && max == "") {
        deleteUrl("price")
    } else {
        (min == "") ? min = 0 : null;
        (max == "") ? max = 0 : null;
        setUrl("price", `${min}#${max}`)
    }
})

//篩選點擊事件:烘焙程度，處理法
$('.filter input[type="checkbox"]').on('click', function () {
    let colElem = $(this).closest('.dropdown').find('button').text().trim()
    if (colElem == "烘焙程度") {
        setCheckboxUrl("baking", this)
    } else if (colElem == "處理法") {
        setCheckboxUrl("method", this)
    }
})

//篩選輸入事件:味道
$('.filter input[type="range"]').on('input', function () {
    //當range的value更動時，顯示其值在對應的span裡和input的樣式
    $(this).next('span').text($(this).val());
    var progress = ($(this).val() - $(this).prop('min')) / ($(this).prop('max') - $(this).prop('min')) * 100;
    $(this).css('background', `linear-gradient(to right, #2E724A ${progress}%, #b6d7a8 ${progress}%)`)
})
$('.filter input[type="range"]').on("change", function () {
    //設定問號參數map
    if ($(this).val() == 1) {
        deleteUrl($(this).prop('id'))
    } else {
        setUrl($(this).prop('id'), $(this).val())
    }
})

//------------------SubFunction-------------------

/* 
 * Cart
 */

/**
 * Uom數量更新
 * @param {number} uom 數量
 * @param {any} self 加減號按鈕元素
 * @param {any} input 輸入元素
 */
function Update_Btn_Uom(uom, self, input) {
    if ($(self).text() == "+") {
        // 設定購買上限為10
        if (uom == 10) {
            $(input).val(uom)
        } else {
            $(input).val(uom + 1)
        }
    }
    if ($(self).text() == "-") {
        // 設定購買下限為1
        if (uom == 1) {
            $(input).val(uom)
        } else {
            $(input).val(uom - 1)
        }
    }
}

/**
 *  X 把資料加入localstorage
 * @param {object} data 一組包含proID、img、name、price、qty的data物件
 */
function addLs(data) {
    var storage = window.localStorage
    var value = storage.getItem("cart")
    var json = (value != null) ? JSON.parse(value) : []
    var temp = json.find((e) => { return e.proID == data.proID })
    if (temp != undefined) {
        temp.qty += data.qty
    } else {
        json.push(data)
    }
    storage.setItem("cart", JSON.stringify(json))
}

/**
 *  X 把資料移出localstorage
 * @param {string} proID 要移出之物件的proID
 */
function removeLs(proID) {
    var storage = window.localStorage
    var strValue = storage.getItem("cart")
    var objValue = JSON.parse(strValue)
    var resultValue = objValue.filter((e) => e.proID != proID)
    storage.setItem("cart", JSON.stringify(resultValue))
}

/**
 *  X 更新localstorage內指定資料的qty
 * @param {object} data 一組包含proID、img、name、price、qty的data物件
 */
function updateLs(data) {
    var storage = window.localStorage
    var strValue = storage.getItem("cart")
    var objValue = JSON.parse(strValue)
    var value = objValue.find((e) => e.proID == data.proID)
    value.qty = data.qty
    addLs(value)
}

/**
 *  X 取得購物籃產品的部分檢視
 * @param {object} data 一組包含proID、img、name、price、qty的data物件
 */
function getHtml(data) {
    $.ajax({
        url: "/List/AddCartItemToLayout",
        method: "POST",
        data: data,
        success: function (data) {
            $('#layout-target').append(data);
        }
    });
}

/**
 * X 把購物籃產品的部分檢視加入購物籃
 * @param {object} data 一組包含proID、img、name、price、qty的data物件
 */
function addHtml(data) {
    var carts = $('#layout-target').find('.card-title')
    var cart = carts.filter(function () {
        return $(this).data('id') == data.proID
    })
    if (cart.length != 0) {
        var newQty = parseInt($(cart).parent('.card-body').find('input[type="number"]').val()) + data.qty
        $(cart).parent('.card-body').find('input[type="number"]').val(newQty)

    } else {
        getHtml(data)
    }
}

/**
 *  X 把資料放入localstorage並且把購物籃產品加入購物籃
 * @param {object} data 一組包含proID、img、name、price、qty的data物件
 */
function setLsHtml(data) {
    addLs(data)
    addHtml(data)
}


/*
 * SetUI n getAjax
 */


function setUI() {
    setBreadcrumb()
    setCheckbox("baking")
    setCheckbox("method")
    setRange()
    setPage()
    setSortNItems()
}

function setBreadcrumb() {
    $('.breadcrumb').empty()
    var pathname = window.location.pathname.split('/')
    $('.breadcrumb').append('<li class="breadcrumb-item"><a href="/Home/Index">首頁</a></li>')
    if (pathname.length > 3) {
        $('.breadcrumb').append(`<li class="breadcrumb-item">${decodeURI(pathname[2])}</li>`)
        $('.breadcrumb').append(`<li class="breadcrumb-item">${decodeURI(pathname[3])}</li>`)
    } else if (pathname.length > 2) {
        $('.breadcrumb').append(`<li class="breadcrumb-item">${decodeURI(pathname[2])}</li>`)
    } else {
        $('.breadcrumb').append(`<li class="breadcrumb-item">所有商品</li>`)
    }
}
function setCheckbox(key) {
    $(`.filter-${key}-item input[type="checkbox"]`).prop("checked", false)
    var queryMap = new URLSearchParams(window.location.search);
    if (queryMap.has(key)) {
        var valueArray = queryMap.get(key).split("#")
        $(`.filter-${key}-item input[type="checkbox"]`).each(function () {
            var text = $(this).closest('.CBcontainer').text()
            var checked = valueArray.some(function (val) { return val == text })
            $(this).prop("checked", checked)
        })
    }
}
function setRange() {
    var queryMap = new URLSearchParams(window.location.search);
    $('.filterTasteItem input[type="range"]').each(function () {
        if (queryMap.has($(this).prop('id'))) {
            var value = queryMap.get($(this).prop('id'))
            $(this).next('span').text(value)
            $(this).val(value)
            var progress = ($(this).val() - 1) / 4 * 100;
            $(this).css('background', `linear-gradient(to right, #2E724A ${progress}%, #b6d7a8 ${progress}%)`);
        } else {
            $(this).next('span').text(1)
            $(this).val(1)
            $(this).css('background', `linear-gradient(to right,#2E724A 0%, #b6d7a8 0%)`);
        }
    })
}
function setPage() {
    var queryMap = new URLSearchParams(window.location.search);
    var totalItem = parseInt($(".ItemSort > div:first").text())
    var pageItem = (queryMap.has("item")) ? queryMap.get("item") : 12
    let pageNum = Math.ceil(totalItem / pageItem) // 頁數的數量 = 產品總數量/每頁顯示數量

    // 根據頁數的數量生成分頁標籤
    $('.pageul').empty()
    for (let i = 1; i <= pageNum; i++) {
        $('.pageul').append(`<li><a>${i}</a></li>`)
    }

    // 設定css
    $('.pageul li').removeClass('active')
    if (queryMap.has("page")) {
        var page = $('.pageul li a').filter(function () { return $(this).text() == queryMap.get("page") })
        // ??? var page = $('.pageul li a').filter((e) => $(e).text() == queryString.get("page"))
        $(page).parent().addClass('active')
    } else {
        $('.pageul>:first').addClass('active')
    }

    // 分頁點擊事件
    $('.pageul li a').on('click', function (e) {
        // 設定問號參數map
        setUrl("page", $(this).text())
    })
}

function setSortNItems() {
    var queryMap = new URLSearchParams(window.location.search);
    if (queryMap.get("sort") == null) {
        $('.priceSort li').closest('div').find('button').text("綜合")
    } else {
        if (queryMap.get("sort") == "desc") {
            $('.priceSort li').closest('div').find('button').text("價格:由低到高")
        }
        else if (queryMap.get("sort") == "asc") {
            $('.priceSort li').closest('div').find('button').text("價格:由高到低")
        }
    }
    if (queryMap.get("item") == null || queryMap.get("item") == "12") {
        $('.itemShow li').closest('div').find('button').text("每頁顯示12個")
    } else if (queryMap.get("item") == "18") {
        $('.itemShow li').closest('div').find('button').text("每頁顯示18個")
    }
}

function setUrl(key, value) {
    var queryMap = new URLSearchParams(window.location.search);
    queryMap.set(key, value)
    history.pushState({ key: "set" }, "", window.location.origin + window.location.pathname + "?" + queryMap)
    getProData(getAjaxUrl())
}
function deleteUrl(key) {
    var queryMap = new URLSearchParams(window.location.search);
    queryMap.delete(key)
    var q = (queryMap.size == 0) ? "" : "?";
    history.pushState({ key: "delete" }, "", window.location.origin + window.location.pathname + q + queryMap)
    getProData(getAjaxUrl())
}
function setCheckboxUrl(key, self) {
    let CheckedArr = []
    let List = $(self).closest('.dropdown').find('input[type="checkbox"]:checked')
    if (List.length == 0) {
        deleteUrl(key)
    } else {
        List.each(function () {
            CheckedArr.push($(this).closest('.CBcontainer').text())
        })
        setUrl(key, CheckedArr.join('#'))
    }
}
function getAjaxUrl() {
    var queryMap = new URLSearchParams(window.location.search);
    var q = (queryMap.size == 0) ? "" : "?";
    var pathname = window.location.pathname.split('/')
    var ajaxurl = ""
    if (pathname.length < 4) {
        ajaxurl = window.location.origin + `/List/Query/${(pathname[2] != null) ? pathname[2] : "所有商品"}` + q + queryMap
        return ajaxurl
    } else {
        pathname.splice(2, 0, "Query")
        var newpathname = pathname.join('/')
        ajaxurl = window.location.origin + newpathname + q + queryMap
        return ajaxurl
    }
}
function getProData(queryString) {
    $.ajax({
        url: queryString,
        method: "GET",
        success: function (jsonData) {
            $(".ItemSort > div:first").text(jsonData.totalCount + " 項")
            $('.cardContainer').empty()
            setPage()
            for (let i = 0; i < jsonData.products.length; i++) {
                let doc =
                    ` <div class="col-sm-4 col-6">
                    <div class="card">
                        <div class="cardImgBody">
                            <img src=${jsonData.products[i].img} class="card-img-top">
                            <div>
                                <a href="/Detail/Detail/${jsonData.products[i].id}">
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="#464646" width="32" height="32" class="bi bi-share" viewBox="0 0 16 16">
                                        <path d="M13.5 1a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3M11 2.5a2.5 2.5 0 1 1 .603 1.628l-6.718 3.12a2.5 2.5 0 0 1 0 1.504l6.718 3.12a2.5 2.5 0 1 1-.488.876l-6.718-3.12a2.5 2.5 0 1 1 0-3.256l6.718-3.12A2.5 2.5 0 0 1 11 2.5m-8.5 4a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3m11 5.5a1.5 1.5 0 1 0 0 3 1.5 1.5 0 0 0 0-3" />
                                    </svg>
                                </a>
                                <div data-id="${jsonData.products[i].productId}" onclick="cardBtnAdd(this)">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="#464646" class="bi bi-cart" viewBox="0 0 16 16">
                                        <path d="M0 1.5A.5.5 0 0 1 .5 1H2a.5.5 0 0 1 .485.379L2.89 3H14.5a.5.5 0 0 1 .491.592l-1.5 8A.5.5 0 0 1 13 12H4a.5.5 0 0 1-.491-.408L2.01 3.607 1.61 2H.5a.5.5 0 0 1-.5-.5M3.102 4l1.313 7h8.17l1.313-7zM5 12a2 2 0 1 0 0 4 2 2 0 0 0 0-4m7 0a2 2 0 1 0 0 4 2 2 0 0 0 0-4m-7 1a1 1 0 1 1 0 2 1 1 0 0 1 0-2m7 0a1 1 0 1 1 0 2 1 1 0 0 1 0-2" />
                                    </svg>
                                </div>
                            </div>
                        </div>
                        <div class="cardImgBody2">
                            <a href="/Detail/Detail/${jsonData.products[i].id}"><img src=${jsonData.products[i].img} class="card-img-top"></a>
                        </div>
                        <div class="card-body">
                            <h5 class="card-title text-center">${jsonData.products[i].productName}</h5>
                            <p class="card-text text-center">NT.${jsonData.products[i].price}</p>
                        </div>
                    </div>
                  </div>`
                $('.cardContainer').append(doc)
            }
        }
    })
}

/**
 * RWD
 */

/**
 * 當視窗大小改變時的處理函數
 * @param {any} e 媒體查詢條件
 * @returns
 */
function handleMediaChange(e) {
    var accordion = document.querySelector('.accordion');
    var filter = document.querySelector('.filter');
    if (accordion == null) {
        return
    }
    if (e.matches) {
        accordion.classList.add('collapse'); // 新增類別
        filter.classList.add('collapse');
    } else {
        accordion.classList.remove('collapse'); // 移除類別
        filter.classList.remove('collapse');
    }
}


/**
 * other
 */

/**
 * 根據頁面滾動位置顯示或隱藏至頂按鈕
 */
function scrollFunction() {
    if (
        document.body.scrollTop > 200 ||
        document.documentElement.scrollTop > 200
    ) {
        $('.topbtn').show()
    } else {
        $('.topbtn').hide()
    }
}

/**
 * 點擊產品浮窗外區域關閉浮窗
 * @param {any} e
 */
function closeModal(e) {
    if ($('.modalFixed')[0] == e.target) {
        $('.modalFixed').remove()
    }
}