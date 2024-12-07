// ------------------------------------------------------- 登入按鈕 ------------------------------------------------------- //

$('.LoginBtn').on('click', function () {
    console.log(12648)

    const UserId = $('#InputUserId').val();
    const Password = $('#InputPassword').val();

    // 傳送資料到後端
    $.ajax({
        url: '/AccountJp/Login',
        type: 'POST',
        data: {
            userId: UserId,
            password: Password
        },
        success: function (data) {
            console.log(data)
            if (data === "0") {
                alert('ログイン完了！お買い物をお楽しみください！');
                window.location.href = '/Home/IndexJp';  
            } else if (data === "1") {
                alert('パスワードが一致しません。もう一度お試しください。');
<<<<<<< HEAD
                
=======
>>>>>>> a2976f9f57be87a58d94b8959e49d7b63412a88e
            } else if (data === "2") {
                alert('まだ会員登録がお済みではありません。こちらから登録手続きをお願いします。');
                window.location.href = '/AccountJp/Register';  
            }
        },
        error: function () {
            alert('エラー');
        }
    });
});


// ------------------------------------------------------- 註冊按鈕 ------------------------------------------------------- //
$('.RegisterBtn').on('click',function () {
    window.location.href = "/AccountJp/Register";
})