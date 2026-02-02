@tool
extends Object

static func AddAutoloadSingleton(plugin: EditorPlugin, name: String, path: String) -> void:
    plugin.add_autoload_singleton(name, path)

static func RemoveAutoloadSingleton(plugin: EditorPlugin, name: String) -> void:
    plugin.remove_autoload_singleton(name)

static func GetSetting(name: String, defaultValue: Variant) -> Variant:
    return ProjectSettings.get_setting(name, defaultValue)

static func Save() -> void:
    ProjectSettings.save()
