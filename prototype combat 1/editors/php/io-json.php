<?php
	session_start();
	ini_set("display_errors",0);
	error_reporting(0);


	//save map
	if(isset($_POST['mapName']) && isset($_POST['map'])){
		if($f = fopen('../map/maps/'.$_POST['mapName'].'.json', 'w+')){
			fputs($f, str_replace("\\","",$_POST['map']));
			echo json_encode(array('status' => 1, 'mapName' => $_POST['mapName'], 'map' => $_POST['map']));
		}
		else{
			echo json_encode(array('status' => -1));
		}
	}


	//read map
	if(isset($_POST['getHistory']) && isset($_POST['name']) && $_POST['getHistory']){
		$fname = md5($_POST['name'].$_SESSION['pseudo']);
		$fname2 = md5($_SESSION['pseudo'].$_POST['name']);
		if(file_exists('users/'.$fname.'.json') || file_exists('users/'.$fname2.'.json')){
			if($f = fopen('users/'.$fname.'.json', 'r')){
				echo file_get_contents('users/'.$fname.'.json');
			}
			else if($f = fopen('users/'.$fname2.'.json', 'r')){
				echo file_get_contents('users/'.$fname2.'.json');
			}
			else{
				echo "Fail openning history !";
			}
		}
		else{
			if($f = fopen('users/'.$fname.'.json', 'w')){
				fputs($f, '{ "filename":"'.$fname.'.json","taille":"0","conv":[]}');
			}
		}
	}
?>