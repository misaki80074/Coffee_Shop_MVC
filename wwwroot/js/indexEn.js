$(window).on('load', function () {
    if (window.innerWidth > 768) {
        forSwiper();
    }
    else {
        RWDprod();
    }

    // 監聽視窗寬度，依照對應的大小改變商品區的呈現狀況
    $(window).on("resize", function () {
        //console.log(window.innerWidth);

        if (!document.querySelector('.swiper') && window.innerWidth > 768) {
            $('#recommendProd').remove();

            $('.recommend').append(`
                <div class="swiper">
                <div class="swiper-wrapper">
                    <div class="swiper-slide"><a href="/DetailEn/DetailEn/1"><img src="/img/coffee_1.jpg"></a></div>
                    <div class="swiper-slide"><a href="/DetailEn/DetailEn/2"><img src="/img/coffee_2.jpg"></a></div>
                    <div class="swiper-slide"><a href="/DetailEn/DetailEn/3"><img src="/img/coffee_3.jpg"></a></div>
                    <div class="swiper-slide"><a href="/DetailEn/DetailEn/4"><img src="/img/coffee_4.jpg"></a></div>
                    <div class="swiper-slide"><a href="/DetailEn/DetailEn/5"><img src="/img/coffee_5.jpg"></a></div>
                    <div class="swiper-slide"><a href="/DetailEn/DetailEn/6"><img src="/img/coffee_6.jpg"></a></div>
                    <div class="swiper-slide"><a href="/DetailEn/DetailEn/7"><img src="/img/coffee_7.jpg"></a></div>
                    <div class="swiper-slide"><a href="/DetailEn/DetailEn/8"><img src="/img/coffee_8.jpg"></a></div>
                </div>
            </div>`);


            forSwiper();
        }

        else if (!document.getElementById("recommendProd") && window.innerWidth <= 768) {
            console.log("小於768");
            RWDprod();
        }


    })

    // map 互動
    $('.app-svg-map--world__name').on('mouseover', function (e) {
        //console.log("OK");

        $("main").append(`<div class="newProd"></div>`)

        // 獲取當前元素的 data-title 屬性
        const countryTitle = $(this).data("title");

        // 呼叫 ajax apple
        apple(countryTitle);

        let coordinate_x = $(this).offset().top - 80;
        let coordinate_y = $(this).offset().left - 10;

        //設定 div 的 CSS
        $('.newProd').css({
            "position": "absolute",
            "top": coordinate_x,
            "left": coordinate_y,
            "background-color": "white",
            "width": "260px",
            "height": "auto",
            "border": "1px solid #2e724a",
            "font-size": "18px",
            "BorderRadius": "3px"
        })


        // 當 text 觸發到 hover 時，相對應的 path 也跟著改變顏色
        let countryIdList = $('#world_map path[id]');

        for (let i = 0; i < countryIdList.length; i++) {
            if (countryIdList[i].id == countryTitle) {
                let countryId = countryIdList[i].id;
                $(`#${countryId}`).css("fill", "#2e724a");
            }
        }

    })

    $('.app-svg-map--world__name').on('mouseout', function () {
        $(".newProd").remove();

        // 當 text 解除 hover 時，相對應的 path 也跟著改回原本的顏色
        const countryTitle = $(this).data("title");
        let countryIdList = $('#world_map path[id]');

        for (let i = 0; i < countryIdList.length; i++) {
            if (countryIdList[i].id == countryTitle) {
                let countryId = countryIdList[i].id;
                $(`#${countryId}`).css("fill", "rgba(224, 234, 224, 1)");
            }
        }
    })

})

// ajax 取得 country 後回傳 div，並顯示在畫面上
function apple(country) {
    console.log(country);

    $.ajax({
        url: "/Home/ProductDataEn",
        type: "POST",
        data: { country: country },
        success: function (data) {
            console.log(data);

            let prodName = data.productName;
            let countryName = data.country;
            let prodImg = data.img;

            // 新增 div，後續把資料庫帶出的資料放在這邊
            const prodInfo = $("<div class='prodInfo'>").html(`
                <img src="${prodImg}"></img>
                <p>${countryName} - ${prodName}</p>`);

            // 將 div 加到 newProd 中
            $(".newProd").append(prodInfo);

            // 設定 newProd 的 CSS
            $(".prodInfo").css({
                "display": "flex",
                "FlexDirection": "row",
                "padding": "3px 3px 3px 6px"
            })

            $('.prodInfo img').css({
                "width": "25%"
            })

            $('.prodInfo p').css({
                "margin-left": "3px"
            })
        }
    })
}

// Swiper 輪播設定
function forSwiper() {

    var swiper = new Swiper('.swiper', {
        direction: 'horizontal',
        slidesPerView: 4,
        loop: true,
        autoplay: {
            delay: 5000
        }

    });
}

// RWD 狀況下的商品區設定
function RWDprod() {
    $('.swiper').remove();

    $('.recommend').append(`<div id="recommendProd"></div>`);
    $('#recommendProd').append(`
        <div class="prodDiv"><a href="/DetailEn/DetailEn/1"><img src="/img/coffee_1.jpg"></a></div>
        <div class="prodDiv"><a href="/DetailEn/DetailEn/2"><img src="/img/coffee_2.jpg"></a></div>
        <div class="prodDiv"><a href="/DetailEn/DetailEn/3"><img src="/img/coffee_3.jpg"></a></div>
        <div class="prodDiv"><a href="/DetailEn/DetailEn/4"><img src="/img/coffee_4.jpg"></a></div>
        <div class="prodDiv"><a href="/DetailEn/DetailEn/5"><img src="/img/coffee_5.jpg"></a></div>
        <div class="prodDiv"><a href="/DetailEn/DetailEn/6"><img src="/img/coffee_6.jpg"></a></div>
        <div class="prodDiv"><a href="/DetailEn/DetailEn/7"><img src="/img/coffee_7.jpg"></a></div>
        <div class="prodDiv"><a href="/DetailEn/DetailEn/8"><img src="/img/coffee_8.jpg"></a></div>
        `);

    // CSS 設定
    $('#recommendProd').css({
        "display": "flex",
        "FlexDirection": "column",
        "JustifyContent": "center",
        "AlignItems": "center"
    });

    $('.prodDiv').css({
        "width": "280px",
        "height": "280px",
        "overflow": "hidden",
        "BorderRadius": "10px",
        "margin": "15px 0"
    });

    $('.prodDiv img').css({
        "width": "280px",
        "BorderRadius": "10px"
    });

    $('.prodDiv img').on('mouseenter', function () {
        $(this).css("transform", "scale(1.2, 1.2)")
    }).on("mouseleave", function () {
        $(this).css("transform", "scale(1, 1)")
    });
}

