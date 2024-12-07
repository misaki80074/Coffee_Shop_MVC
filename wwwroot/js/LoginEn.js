// ------------------------------------------------------- 登入按鈕 ------------------------------------------------------- //

$('.LoginBtn').on('click', function () {
    console.log(12648)

    const UserId = $('#InputUserId').val();
    const Password = $('#InputPassword').val();

    // 傳送資料到後端
    $.ajax({
        url: '/AccountEn/Login',
        type: 'POST',
        data: {
            userId: UserId,
            password: Password
        },
        success: function (data) {
            console.log(data)
            if (data === "0") {
                alert(`Welcome! Let's go shopping!`);
                window.location.href = '/Home/IndexEn';  
            } else if (data === "1") {
                alert('Invalid password. Please try again.');           
            } else if (data === "2") {
                alert('You need to create an account to continue.');
                window.location.href = '/AccountEn/Register';
            } else if (data === "2") {
                alert('You need to create an account to continue.');
                window.location.href = '/AccountEn/Register';  
            }
        },
        error: function () {
            alert('Error');
        }
    });
});


// ------------------------------------------------------- 註冊按鈕 ------------------------------------------------------- //
$('.RegisterBtn').on('click',function () {
    window.location.href = "/AccountEn/Register";
})