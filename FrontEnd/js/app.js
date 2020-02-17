var authToken = null;
var urlBase="http://localhost:61854/api/";
var userId = "";
var userName = "";
var chatRoomId = "";
var connection = null;

$( document ).ready(function() {
    init();
});

function init()
{
	$('#registerSection').hide();
	$('#loginSection').show();
	$('#createRoomSection').hide();
	$('#joinRoomSection').hide();
	$('#chatSection').hide();
	setButtonActions();
}

function setButtonActions()
{
	$('#btnRegister').click(function() {
		
	});
	$("#btnlogin").click(function(){
		var username = $("#userName").val();
		var password = $("#loginPass").val();		
		$.ajax({
			type: "POST",
			url: urlBase + "auth/login",
			data: JSON.stringify({
				username: username,
				password: password
			}),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (response) {
				authToken = response.token;
				ShowRoomsList();
			},
			fail: function(xhr, textStatus, errorThrown){
				alert('request failed');
			}
		});
	} );
	$('#btnCreate').click(function() {
	  $.ajax({
			type: "POST",
			url: urlBase + "ChatRooms/Create",
			headers: {'Authorization': 'Bearer ' + authToken},
			dataType: "json",
			data: JSON.stringify({
				name: $("#roomName").val(),
				id: 0
			}),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (response) {
				token = response.token;
				GetChatrooms();
			},
			fail: function(xhr, textStatus, errorThrown){
				alert('request failed');
			}
		});
	});
	$('#btnJoin').click(function() {
		chatroomId=$('#chatroomSelection').val()
	  $.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		headers: {'Authorization': 'Bearer ' + authToken},
		dataType: "json",
		data: JSON.stringify({chatroomId: parseInt($('#chatroomSelection').val(),10)}),
		url: urlBase + "Chatrooms/JoinToChat",
		success: function (response) {
			Connect(authToken);		
			var messages=$('#messages');
			messages.html('');
			GetChatRoomsMessages();
			$('#chatSection').show();
			},
		});
		
	});	
}

function GetChatRoomsMessages(){
	$.ajax({
		type: "GET",
		url: urlBase + "ChatRooms/GetChatRoomMessage?id="+chatroomId,
		contentType: "application/json; charset=utf-8",
		headers: {'Authorization': 'Bearer ' + authToken},
		dataType: "json",
		success: function (data) {  
			$.each(data, function( index, value ) {
				var encodedName = value.userName;
				var encodedMsg = value.message;
				var liElement = document.createElement('li');
				liElement.innerHTML = '<strong>' + encodedName + '</strong>:&nbsp;&nbsp;' + encodedMsg;
				document.getElementById('messages').appendChild(liElement);
			});
			$('#messages').animate({scrollTop: $('#messages').prop("scrollHeight")}, 500);
		}			
	});
	
}

function toggleLogin(toggleLogin)
{
	if(toggleLogin>0)
	{
		$('#registerSection').toggle();
		$('#loginSection').toggle();
	}else
	{
		$('#loginSection').toggle();
		$('#registerSection').toggle();
	}
}

function ShowRoomsList(){
				$("#loginSection").toggle()
				GetChatrooms();
				$('#createRoomSection').toggle();
				$('#joinRoomSection').toggle();
			};
function GetChatrooms() {
				var selectRooms = $("#chatroomSelection");
				selectRooms.empty();
				$.ajax({
					type: "GET",
					url: urlBase + "ChatRooms/GetChatRooms",
					contentType: "application/json; charset=utf-8",
					headers: {'Authorization': 'Bearer ' + authToken},
					dataType: "json",
					success: function (data) {  
						selectRooms.append('<option value="-1">Select a room</option>');
						$.each(data, function( index, value ) {
						  selectRooms.append('<option value="' + value.id + '">' + value.name + '</option>');
						});
					}				
				});
			};

var Connect = function(){			
	if(connection==null){
		connection = new signalR.HubConnectionBuilder().withUrl(urlBase + "chat",
		{
			accessTokenFactory: () => authToken 
		}).build();		
	}		
	var messageInput = document.getElementById('message');
	 connection.on(chatroomId,function (senderId,messageBody) {
                if(messageBody.messageSent){
					var encodedName = senderId;
					var encodedMsg = messageBody.messageSent;
					var liElement = document.createElement('li');
					liElement.innerHTML = '<strong>' + encodedName + '</strong>:&nbsp;&nbsp;' + encodedMsg;
					document.getElementById('messages').appendChild(liElement);
					$('#messages').animate({scrollTop: $('#messages').prop("scrollHeight")}, 500);
				}				
            });
            connection.start()
                .then(function () {										
                    document.getElementById('send').addEventListener('click', function (event) {
						var messageBody = $('#message').val();
						var user = parseJwt(authToken);
                        connection.invoke('send', parseFloat(chatroomId),messageBody,user.unique_name);
                        messageInput.value = '';
                        messageInput.focus();
                        //event.preventDefault();
                    });
					connection.invoke('JoinRoom', chatroomId);
            })
            .catch(error => {
                console.error(error.message);
            });
	
}
function parseJwt (token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
};