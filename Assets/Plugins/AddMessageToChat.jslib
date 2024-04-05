mergeInto(LibraryManager.library, {
	AddMessageToChat: function(message){
		//document.getElementById('myFrame').contentWindow.vectorcoord1 = c1;
		//document.getElementById('myFrame').contentWindow.vectorcoord1 = c2;
		//document.getElementById('myFrame').contentWindow.vectorcoord1 = c3;
		console.log("calling AddMessageToChat() from sim");
		var chatMessage = UTF8ToString(message);
       SendUnityChatMessage(chatMessage);
	},
	
});