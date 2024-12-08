// ------------------------------------------------------------取得會員資料  **缺訂單資訊
$.ajax({
    url: '/Account/GetMemberInfo',
    type: 'GET',
    dataType: 'json',
    success: function (data) {
        console.log(data);
        //console.log(data.customer[0].name);

        $('#InputName').val(data.customer[0].name);
        $('#InputPhone').val(data.customer[0].phone);
        $('#InputEmail').val(data.customer[0].email)
        $('#InputAddress').val(data.customer[0].receiverAddress);

        // 訂單資料
        const orderheaders = data.orderheaders;

        // 顯示訂單資料
        const ordersList = $('#nav-order .table tbody');
        ordersList.empty();  // 清空原有的訂單資料
        orderheaders.forEach(order => {

            // 格式化日期
            const orderDateParts = order.orderDate.split(' ');
            //console.log(orderDateParts);
            const formattedDate = new Date(`${orderDateParts[3]}-${orderDateParts[0]}-${orderDateParts[2]}`).toLocaleDateString();
            //console.log(formattedDate);

            const orderRow = `
                <tr>
                    <td scope="row" class="text-center">${order.orderId}</td>
                    <td class="text-center">${formattedDate}</td>
                    <td class="text-center">${order.total}</td>
                    <td class="text-center">${order.statusName}</td>
                    <td><button class="OrderCheckBtn" data-bs-toggle="modal" data-bs-target="#OrderCheckModal"  data-orderid="${order.orderId}">查閱</button></td>
                </tr>
            `;

            
            ordersList.append(orderRow);

            $('.OrderCheckBtn').css({
                "backgroundColor": "#2e724a",
                "border": "1px solid #2e724a",
                "padding": "7px 10px",
                "border-radius": "5px",
                "color": "white",
                "font-size": "18px"
            })

            $('.OrderCheckBtn').on('mouseenter', function () {
                $(this).css({
                    "background-color": "white",
                    "color": "#2e724a"
                })
            }).on('mouseleave', function () {
                $(this).css({
                    "backgroundColor": "#2e724a",
                    "border": "1px solid #2e724a",
                    "padding": "7px 10px",
                    "border-radius": "5px",
                    "color": "white",
                    "font-size": "18px"
                })
            })

            $('.text-center').css({
                "vertical-align": "middle",
                "text-align": "center",
                "font-size": "18px"
            })
        });

        $('.OrderCheckBtn').on('click', function () {
            var orderId = $(this).data('orderid');
            console.log(orderId)
            $.ajax({
                url: '/Account/GetOrderDetail',  
                type: 'POST',
                data: { orderid: orderId },  
                success: function (response) {
                    console.log(response.total[0])
                    // 當成功取得資料後，顯示訂單明細
                    var orderdetails = response.orderdetails;
                    var total = response.Total;
                    //console.log(orderdetails)
                    // 清空現有的訂單資料
                    $('.modal-body .orderDetailsTable tbody').empty();

                    // 顯示訂單明細
                    orderdetails.forEach(function (item) {
                        console.log(item)

                        var row = `<tr>
                            <td class="productTd"><img src="${item.img}">
                                <span>${item.productName}</span></td>
                            <td class="text-center">${item.price}</td>
                            <td class="text-center">${item.qty}</td>
                            <td class="text-center">${item.totle}</td>
                        </tr>`;
                        $('.orderDetailsTable tbody').append(row);
                    });

                    $('.text-center').css({
                        "vertical-align": "middle",
                        "text-align": "center",
                        "font-size": "18px"
                    })

                    $('td img').css({
                        "width": "10%",
                        "display": "inline",
                        "margin-left": "80px"
                    })

                    $('.productTd').css({
                        "max-width": "300px",
                        "text-align": "left"
                    })

                    // 顯示總計
                    $('.small-total').text(response.total[0]);  
                    $('.big-total').text(response.total[0]);

                },
                error: function () {
                    alert('無法獲取訂單明細，請稍後再試');
                }
            });
        });
    },
    error: function (xhr, status, error) {
        console.error('錯誤訊息:', error);
        alert('錯誤訊息，看主控台');
    }
});


// ----------------------------------------------------------------------------- 登出按鈕 **完成
$('.SignOutBtn').on('click', function () {
    $.ajax({
        url: '/Account/Logout',
        type: 'GET',
        success: function (response) {
            console.log(response)
            alert(response);                     // 顯示登出成功訊息
            window.location.href = '/Home/Index';
        },
        error: function () {
            alert('登出失敗，請稍後再試');
        }
    });
});

// ----------------------------------------------------------------------------- 取消按鈕 畫面重載
$('.CancelChangeBtn').on('click', function (e) {
    e.preventDefault();  // 防止表單提交
    location.reload();   // 重載頁面
});


// -----------------------------------------------------------------------------儲存變更按鈕 檢查送後段
$('.SaveChangeBtn').on('click', function (e) {
    e.preventDefault();         // 防止表單提交

    // 取得表單資料
    var name = $('#InputName').val();
    var phone = $('#InputPhone').val();
    var email = $('#InputEmail').val();
    var address = $('#InputAddress').val();

    // 手機格式檢查
    var phonePattern = /^09\d{8}$/;
    if (!phonePattern.test(phone)) {
        alert("請輸入正確的手機號碼格式！");
        return;
    }

    // mail格式檢查
    var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    if (!emailPattern.test(email)) {
        alert("請輸入正確的電子郵件格式！");
        return;
    }

    //$.ajax({
    //    type: "GET",
    //    url: "/Account/GetUserid",

    //    success: function (response) {
    //        console.log("UserId:", response.userid);
    //    },
    //    error: function () {
    //        alert("錯誤");
    //    }
    //});

    // 先取userid
    $.ajax({
        type: "GET",
        url: "/Account/GetUserid",
        success: function (response) {
            if (response.userid) {
                var userid = response.userid;
                console.log("取得的 UserId:", userid);

                // 再送更新資料的請求
                $.ajax({
                    type: "POST",
                    url: "/Account/UpdateMemberInfo",
                    data: {
                        userid: userid,
                        name: name,
                        phone: phone,
                        email: email,
                        address: address
                    },
                    success: function (response) {
                        alert(response);        // 顯示後端返回的成功訊息
                        location.reload();

                    },
                    error: function (xhr, status, error) {
                        alert("更新失敗，請稍後再試！");
                    }
                });
            } else {
                alert("無法取得 UserId，請重新登入！");
            }
        },
        error: function (xhr, status, error) {
            alert("取得 UserId 失敗");
        }
    });

});

// -------------------------------------------------------------- 更新密碼的部分
$('.UpdateBtn').on('click', function (e) {
    e.preventDefault();          // 防止表單默認提交行為

    // 取得輸入的值
    var currentPwd = $('#CurrentPwd').val();
    var newPwd = $('#NewPwd').val();
    var checkNewPwd = $('#CheckNewPwd').val();

    // 檢查新密碼與確認新密碼是否一致
    if (newPwd !== checkNewPwd) {
        alert("新密碼與確認新密碼不一致！");
        return;
    }

    // 檢查新密碼格式（至少8個字元，包含大小寫字母與數字）
    var pwdPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$/;
    if (!pwdPattern.test(newPwd)) {
        alert("新密碼必須包含大小寫字母與數字，且至少8個字元！");
        return;
    }

    // 先取得 UserId
    $.ajax({
        type: "GET",
        url: "/Account/GetUserid",
        success: function (response) {
            console.log("取得的 UserId:", response.userid);

            // 發送更新密碼請求
            $.ajax({
                type: "POST",
                url: "/Account/UpdatePassword",
                data: {
                    userid: response.userid,
                    o_password: currentPwd,
                    n_password: newPwd
                },
                success: function (updateResponse) {
                    console.log(updateResponse)
                    if (updateResponse === "0") {
                        alert("密碼更新成功！");

                        // 新增
                        $.ajax({
                            type: "GET",
                            url: "/Account/Logout",
                            success: function () {
                                window.location.href = '/Account/Login';
                            },
                            error: function () {
                                alert("登出失敗");
                            }
                        });

                        window.location.href = '/Account/Member';
                    } else if (updateResponse === "1") {
                        alert("舊密碼錯誤！");
                    } else {
                        alert("密碼更新失敗，請稍後再試！");
                    }
                },
                error: function () {
                    alert("密碼更新失敗，請稍後再試！");
                }
            });

        },
        error: function () {
            alert("取得 UserId 失敗，請稍後再試！");
        }
    });
});