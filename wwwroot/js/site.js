// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function updateCartAjax(id, quantity, handle) {
	debugger
	$.ajax({
		url: '/api/cart/addtocart',
		type: 'POST',
		contentType: 'application/json',
		data: `{ "productid": ${id}, "quantity": ${quantity}}`,
		success: function (response) {
			handle(response);
		},
		error: function (xhr, status, error) {
			showToast("Send api fail");
		}
	});
}
function getQRCode(amount) {
	debugger
	$.ajax({
		url: '/api/cart/getqrcode',
		type: 'POST',
		contentType: 'application/json',
		data: `{ "amount": ${id} }`,
		success: function (response) {
			
		},
		error: function (xhr, status, error) {
			showToast("Send api fail");
		}
	});
}

function showToast(message) {
	Toastify({
		text: message,
		duration: 3000,
		close: true
	}).showToast();
}

var $registerBtns = $("a.register-btn");
var $loginBtns = $("a.login-btn");
var $registerPopup = $("#register-form");
var $loginPopup = $("#login-form");
var $closeBtns = $(".close-btn");

// Show/hide popups
var showRegisterPopup = () => {
	hideLoginPopup();
	$registerPopup.show();
};

var hideRegisterPopup = () => {
	$registerPopup.hide();
};

var showLoginPopup = () => {
	hideRegisterPopup();
	$loginPopup.show();
};

var hideLoginPopup = () => {
	$loginPopup.hide();
};

// Attach event listeners
$registerBtns.on("click", showRegisterPopup);
$loginBtns.on("click", showLoginPopup);
$closeBtns.on("click", hideRegisterPopup);
$closeBtns.on("click", hideLoginPopup);

// Close popups when clicking outside
$(window).on('click', (event) => {
	if ($registerPopup.is(event.target)) {
		hideRegisterPopup();
		hideLoginPopup();
	}
});

$("#login").on("submit", (e) => {
	e.preventDefault();
	Login();
});

$("#register").on("submit", (e) => {
	e.preventDefault();
	Register();
});

function ShowError(message) {
	Toastify({
		text: message,
		duration: 3000,
		close: true,
		gravity: "top",
		position: "right",
		stopOnFocus: true,
		style: {
			background: "#e74c3c",
		}
	}).showToast();
}

function Login() {
	debugger
	var email = document.getElementById("login_email").value;
	var password = document.getElementById("login_password").value;

	$.ajax({
		url: '/api/Auth/CheckLogin',
		type: 'POST',
		data: `{ "email": "${email}", "password": "${password}" }`,
		contentType: "application/json",
		success: function (response) {
			var status = response;

			if (!status) {
				ShowError("Username or Password is incorect!");
			} else {
				document.getElementById("login").submit();
			}
		},
		error: function (xhr, status, error) {
			ShowError("Fail to connect server");
		}
	});
}

function Register() {
	var email = document.getElementById("register_email").value;

	$.ajax({
		url: '/api/Auth/CheckEmail',
		type: 'POST',
		data: `{ "email": "${email}" }`,
		contentType: "application/json",
		success: function (response) {
			var status = response;

			if (status) {
				ShowError("Username was exist!");
			} else {
				document.getElementById("register").submit();
			}
		},
		error: function (xhr, status, error) {
			ShowError("Fail to connect server");
		}
	});
}

var connection = new signalR.HubConnectionBuilder()
	.withUrl("/noti")
	.build();

connection.start().then(function () {
	console.log("Connect signalr success");
	connection.invoke("JoinRoom");
})

connection.on("UpdateOrder", () => {

	if (window.location.href.includes("billhistory")) {
		var container = $("#dataTable")[0];

		$.ajax({
			url: '/admin/order/index?handler=ListOrders',
			contentType: 'application/html ; charset:utf-8',
			type: 'GET',
			dataType: 'html',
			success: function (result) { container.innerHTML = result }
		});
	}

	showToast("Nhận được đơn hàng mới!");
})