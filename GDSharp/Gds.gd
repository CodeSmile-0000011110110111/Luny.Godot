class_name Gds
extends Object

const RootPath = "res://addons/lunyscript/Luny.Godot/GDSharp/"
const RuntimePath = RootPath + "Runtime/"

const File = preload(RuntimePath + "File.gd")
const Res = preload(RuntimePath + "Resource.gd") # 'Res' to avoid name clash with built-in 'Resource'
const Str = preload(RuntimePath + "String.gd") # 'Str' to avoid name clash with built-in 'String'
