// ------------------------------------------------------- 登入按鈕 ------------------------------------------------------- //

$('.LoginBtn').on('click', function () {
    console.log(12648)

    const UserId = $('#InputUserId').val();
    const Password = $('#InputPassword').val();

    // 傳送資料到後端
    $.ajax({
        url: '/Account/Login',
        type: 'POST',
        data: {
            userId: UserId,
            password: Password
        },
        success: function (data) {
            console.log(data)
            if (data === "0") {
                alert('登入成功，開始購物吧!');
                window.location.href = '/Home/Index';  
            } else if (data === "1") {
                alert('登入失敗，密碼錯誤!'); 
            } else if (data === "2") {
                alert('該帳號未註冊，請註冊!');
                window.location.href = '/Account/Register';
                alert('登入失敗，密碼錯誤!');   
            } else if (data === "2") {
                alert('該帳號未註冊，請註冊!');
                window.location.href = '/Account/Register';
            }
        },
        error: function () {
            alert('伺服器錯誤');
        }
    });
});


// ------------------------------------------------------- 註冊按鈕 ------------------------------------------------------- //
$('.RegisterBtn').on('click',function () {
    window.location.href = "/Account/Register";
})