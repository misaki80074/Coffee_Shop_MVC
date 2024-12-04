function AddCartHTML() {
    // 從 localStorage 讀取購物車資料並轉換成陣列或物件 如果找不到回傳空陣列
    let cart = JSON.parse(localStorage.getItem('cart')) || [];

    // 取得 tbody ，用於插入 cart 的商品資料
    const cartItemsContainer = document.getElementById('shop');
    let totalPrice = 0;
    // 檢查 cart 是否為空陣列
    if (cart.length === 0) {
        const emptyRow = document.createElement('tr');
        const emptyMessage = document.createElement('td');
        emptyMessage.setAttribute('colspan', '7');
        emptyMessage.innerHTML = 'Empty';
        emptyRow.appendChild(emptyMessage);
        cartItemsContainer.appendChild(emptyRow);
    } else {
        $("#defulttext").text("");
        // 如果 cart 不為空，則將資料加入到Table。item為物件(內有name、price、image)，index為索引
        cart.forEach((item, index) => {
            const row = document.createElement('tr');
            row.classList.add('row', 'shoptr')
            //-------------------------------------------------------
            // 存入 OrderDetail item
            // const itemCell = document.createElement('td');
            // const inputNO = document.createElement('input');
            // inputNO.type = 'hidden';
            // inputNO.name = 'itemNO';
            // inputNO.value = index + 1;
            // itemCell.appendChild(inputNO);
            // itemCell.innerHTML += index + 1;
            // row.appendChild(itemCell);
            //-------------------------------------------------------
            // 圖片
            const imgCell = document.createElement('td');
            const inputIMGSRC = document.createElement('input');
            inputIMGSRC.type = 'hidden';
            inputIMGSRC.name = "imgsrc";
            inputIMGSRC.value = item.image;
            const inputIMG = document.createElement('img');
            inputIMG.name = 'itemimg';
            inputIMG.src = item.image;
            inputIMG.style.width = "100px"; // 之後要刪 我一定會忘記
            inputIMG.style.hight = "100px"; // 之後要刪 我一定會忘記
            imgCell.appendChild(inputIMG);
            imgCell.appendChild(inputIMGSRC);
            imgCell.classList.add('col-4')
            row.appendChild(imgCell);
            //-------------------------------------------------------
            // 名稱
            const nameCell = document.createElement('td');
            const inputName = document.createElement('input');
            inputName.type = 'hidden';
            inputName.name = 'itemNane';
            inputName.value = item.name;
            nameCell.appendChild(inputName);
            // -------------------------------------
            const inputProductId = document.createElement('input');
            inputProductId.type = 'hidden';
            inputProductId.name = 'itemId';
            inputProductId.value = item.productId;
            nameCell.appendChild(inputProductId);
            nameCell.classList.add('shopname')
            nameCell.innerHTML += item.name;
            // 插入 name 到 td
            // row.appendChild(nameCell);
            //-------------------------------------------------------
            const QTYCell = document.createElement('td');
            // 新增一個按鈕元素
            const DECButton = document.createElement('button');
            DECButton.type = 'button';
            const inputQTY = document.createElement('input');
            inputQTY.type = 'hidden';
            inputQTY.name = 'itemQTY';
            inputQTY.value = item.qty;
            QTYCell.appendChild(inputQTY);
            DECButton.innerHTML += ' - ';
            DECButton.onclick = () => changeQTY(index, -1); // 調用+-函數
            // 新增一個按鈕元素
            const ADDButton = document.createElement('button');
            ADDButton.type = 'button';
            ADDButton.innerHTML += ' + ';
            ADDButton.onclick = () => changeQTY(index, 1);  // 調用+-函數
            const QTY = document.createElement('span');
            QTY.innerHTML += item.qty;
            QTY.id = `QTY-${index}`; // 設定該 span 的id，函數 changeQTY 使用
            QTYCell.appendChild(DECButton);
            QTYCell.appendChild(QTY);
            QTYCell.appendChild(ADDButton);
            QTYCell.classList.add('shopqty')
            // row.appendChild(QTYCell); // 將結果插入 td
            //-------------------------------------------------------
            //把名稱跟數量包起來
            const nameQty = document.createElement('td');
            nameQty.appendChild(nameCell)
            nameQty.appendChild(QTYCell)
            nameQty.classList.add('col-4', 'nameQty')
            row.appendChild(nameQty)
            //-------------------------------------------------------
            // 單價
            // const priceCell = document.createElement('td');
            // const inputprice = document.createElement('input');
            // inputprice.type = 'hidden';
            // inputprice.name = 'itemprice';
            // inputprice.value = item.price;
            // priceCell.appendChild(inputprice);
            // priceCell.innerHTML += `$${item.price.toLocaleString()}`;
            // row.appendChild(priceCell);
            //-------------------------------------------------------
            // 操作（移除按鈕）
            const actionCell = document.createElement('td');
            const removeButton = document.createElement('button');
            removeButton.type = 'button';
            removeButton.classList.add('delete');
            removeButton.innerHTML = 'X';
            removeButton.onclick = function () {
                removeFromCart(index, item.productId); // 點擊按鈕時調用移除函數
                $(this).closest('tr').remove();
            };
            actionCell.appendChild(removeButton);
            actionCell.classList.add('shopclose')
            // row.appendChild(actionCell);
            //-------------------------------------------------------
            // 總價 (數量 * 單價)
            const totalCell = document.createElement('td');
            const inputtotal = document.createElement('input');
            inputtotal.type = 'hidden';
            inputtotal.name = 'itemtotal';
            inputtotal.value = item.price * item.qty;
            totalCell.appendChild(inputtotal);
            totalCell.innerHTML += `$${(item.price * item.qty).toLocaleString()}`;
            totalCell.id = `total-${index}`;
            totalCell.classList.add('shoptotal')
            // row.appendChild(totalCell);
            //-------------------------------------------------------
            //把移除按鈕和總價包起來
            const closeTotal = document.createElement('td');
            closeTotal.appendChild(actionCell)
            closeTotal.appendChild(totalCell)
            closeTotal.classList.add('col-4', 'closeTotal')
            row.appendChild(closeTotal)
            //-------------------------------------------------------
            // 新增一筆資料 tr
            cartItemsContainer.appendChild(row);
            totalPrice += item.price * item.qty; // 全部總和
        });
    }
    function changeQTY(index, change) {
        // index 第幾個索引、- 還是 +
        if (cart[index].qty + change > 0) {
            cart[index].qty += change;
            document.getElementById(`QTY-${index}`).innerHTML = cart[index].qty;
            document.getElementById(`total-${index}`).innerHTML = `$${(cart[index].price * cart[index].qty).toLocaleString()}`;
            localStorage.setItem('cart', JSON.stringify(cart)); // 將結果重新記錄到 本機儲存
        }
        // document.getElementById('itemtotal').innerHTML.reload;
        // document.location.reload();
    }
    // 移除的 某 項商品
    function removeFromCart(index, ProductID) {
        cart.splice(index, 1); // 移除索引上的這一個元素
        localStorage.setItem('cart', JSON.stringify(cart)); // 更新 localStorage，轉成字串

        //window.location.reload();

        Deletecartitem(ProductID);

    }
    var V_userId = "C11070"; // 使用者登入後的ID

    function Deletecartitem(ProductID) {
        $.ajax({
            url: "/Cart/DeleteCartItem",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Hmodel:
                {
                    UserId: V_userId
                },
                Dmodels:
                {
                    ProductId: ProductID
                }
            })
        });
    }

}

// 改變商品數量
// 清空購物車
function clearCart() {
    // 清空 localStorage 中的購物車資料
    localStorage.removeItem('cart');
    // 重新加載頁面
    document.location.reload();
}
//const totle = document.querySelector(".total-price");
//document.getElementById("totalprice").value = totalPrice;
//totle.append(`總和為: $ ${totalPrice.toLocaleString()}`);
// totle.innerHTML = `總和為: $${totalPrice.toLocaleString()}`;