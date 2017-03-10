	var config = {
    selectors: {
			target:'.mix',
			filter:'.filter',
			sort:'.sort'
		},
		load: {
			//sort:'myorder:desc'
		},
		
		controls: {
			enable:true
			//activeClass:'on'
		},
		
		animation: {
			enable:true,
			effects:'rotateZ stagger',
			duration:1500
		},
		
		/*layout: {
			//display:'block'
			containerClass:'list'
		}*/
		
		callbacks: {
			/*onMixLoad:function(state) {
				alert('Load');
			},*/
			onMixStart:function(state) {
				//alert(state.$targets.text());
			},
			onMixEnd:function() {
				
			}
		}
};
$(document).ready(function()
{	
	/*
	CreateList(20);
	DrawList();
	Find();
	var warn = setInterval('CreateNewList(); Find(); ', 10000);
	*/

});
var names = new Array();
var iqs = new Array();
var names_new = new Array();
var iqs_new = new Array();
var size = 0;

/*
	function Move(a,b)
	{
		var begin = 0;
		var step = 0;
		var end = 0;
		var buffer = 0;
		if (a < b)
		{
			begin = a;
			step = +1;
			end = b;
		}
		else
		{
			begin = a;
			step = -1;
			end = b;	
		}
		var i = begin;
		(function() {
			if (i != end + step + step)
			{
				if (i == end + step)
				{
					$('#'+end).animate({ paddingLeft: 0}, 300);
					$('#'+end+' table').removeClass("current");
				}
				else
				{
					if(i == begin)
					{
					$('#'+i).animate({ paddingLeft: 50}, 150);
					$('#'+i+' table').addClass("current");
					}
					else
					{
					$('#'+i).animate({ paddingLeft: 50}, 150);
					$('#'+i+' table').addClass("current");
					//обмен
					
					buffer = names[(i-step)];
					names[(i-step)] = names[i];
					names[i] = buffer;
					
					buffer = iqs[(i-step)];
					iqs[(i-step)] = iqs[i];
					iqs[i] = buffer;
					
					$('#'+i + ' .name').html(names[i]);
					$('#'+(i-step)+ ' .name').html(names[i-step]);
					
					$('#'+i + ' .iq').html(iqs[i]);
					$('#'+(i-step)+ ' .iq').html(iqs[i-step]);
					
					$('#'+(i-step)).animate({ paddingLeft: 0}, 300);
					$('#'+(i-step)+' table').removeClass("current");
					}
					
				}
				i = i + step;	
				
				setTimeout(arguments.callee, 300);
			}
		}
		)();
	}
	*/
	function CreateListJson(json)
	{
	var objects = JSON.parse(json);
	if (size == 0)
	{
	size = objects.length+1;
	
	names = new Array(size);
	iqs = new Array(size);
	names_new = new Array(size);
	iqs_new = new Array(size);

	for (var i = 1; i < size; i++)
		{
			names[i] = objects[i-1].name;
			iqs[i] = objects[i-1].iQash;
		}
		DrawList();
	}
	if (size != 0)
	{
		for (var i = 1; i < size; i++)
		{
			names_new[i] = objects[i-1].name;
			iqs_new[i] = objects[i-1].iQash;
		}
		Find();
	}
	}


	
	function CreateList(count)
	{
	size = count +1;
	names = new Array(size);
	iqs = new Array(size);
	string = "";
	for (var i = 1; i < size; i++)
		{
			names[i] = "team"+i;
			iqs[i] = (size-i)*25+300;
			string = string + names[i] + ':' + iqs[i] + '   ';
		}
		//alert(string);
		CreateNewList();
	}
	function DrawList()
	{
	var s="";
	for (var i = 1; i < size; i++)
		$('#'+i).remove();
	for (var i = 1; i < size; i++)
		s = s +	'<div class="mix" data-myorder='+iqs[i]+' id='+i+'><table border="0" cellpadding="0" cellspacing="0"  width = "100%" ><tr><th class="arrow"><h1></h1></th><th class="name"><h1>'+names[i]+'</h1></th><th class="add"><h1></h1></th><th class="iq"><h1>'+iqs[i]+'</h1></th></tr></table></div>';
	$('#teames').after(s);
	s =""; 
	for (var i = 1; i < size; i++)
		s = s +	'<div class="mix"><table width = "100%" cellpadding="0" cellspacing="0" border="0"><tr><th class="number"><h1>'+i+'</h1></tr></table></div>';
	$('#number0').after(s);
	$("#Container").mixItUp(config);
	}
	
	function CreateNewList()
	{
	names_new = new Array(size);
	iqs_new = new Array(size);
	var buffer;
	var string="";
	for (var i = 1; i < size; i++)
		{
			names_new[i] = names[i];
			iqs_new[i] = iqs[i];
		}
		iqs_new[Random(1, size)] = 25*Random(1, 100);
		iqs_new[Random(1, size)] = 25*Random(1, 100);
		iqs_new[Random(1, size)] = 25*Random(1, 100);
	}

function Find()
{
	$( '.mix').removeClass("red_background green_background");
	$(' .add h1').html("");
	$(' .arrow h1').html("");
	$( '.mix .arrow').removeClass("green_text red_text");
	$( '.mix .add').removeClass("green_text red_text");
	
	for (var i = 1; i < size; i++ )
	{
		for (var j = 1; j < size; j++)
		{
			if ((names[i] == names_new[j]) && (iqs[i]!=iqs_new[j]))
			{
				if (iqs[i] < iqs_new[j]) 
				{
					$( '#'+i+'.mix').addClass("green_background");
					$('#'+i + ' .add h1').html(iqs_new[j]-iqs[i]);
					$('#'+i + ' .arrow h1').html('⬆');
					$( '#'+i+'.mix .arrow').addClass("green_text");
					$( '#'+i+'.mix .add').addClass("green_text");
				}
				if (iqs[i] > iqs_new[j]) 
				{
					$( '#'+i+'.mix').addClass("red_background");
					$('#'+i + ' .add h1').html(iqs_new[j]-iqs[i]);
					$('#'+i + ' .arrow h1').html('⬇');
					$( '#'+i+'.mix .arrow').addClass("red_text");
					$( '#'+i+'.mix .add').addClass("red_text");
				}
				iqs[i] = iqs_new[j];
				$('#'+i).data('myorder', iqs[i] ).attr('data-myorder', iqs[i] );
				//$('#'+i + ' .iq h1').animate({opacity: 0.1}, 100);
				$('#'+i + ' .iq h1').html(iqs[i]);
				//sleep(100);
				//$('#'+i + ' .iq h1').animate({opacity: 1}, 100);
				
			}
		}
		
	}
	$("#sort").click();
}	

function sleep(milliseconds) {
  var start = new Date().getTime();
  for (var i = 0; i < 1e7; i++) {
    if ((new Date().getTime() - start) > milliseconds){
      break;
    }
  }
}

function Random(min, max)
{
  return Math.floor(Math.random() * (max - min + 1)) + min;
}

function Sort()
{
		var buffer;
		for (var i = 1; i < size; i++)
		for (var j = 1; j < size; j++)
		{
			if (iqs[i] > iqs[j])
			{
					buffer = names[i];
					names[i] = names[j];
					names[j] = buffer;
					
					buffer = iqs[i];
					iqs[i] = iqs[j];
					iqs[j] = buffer;
			}
		}
}