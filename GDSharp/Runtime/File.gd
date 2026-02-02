extends Object

static func Exists(path: String) -> bool:
    return FileAccess.file_exists(path)

static func Open(path: String, flags: FileAccess.ModeFlags) -> FileAccess:
    return FileAccess.open(path, flags)

static func OpenReadable(path: String) -> FileAccess:
    return Open(path, FileAccess.READ)

static func OpenWritable(path: String) -> FileAccess:
    return Open(path, FileAccess.WRITE)

static func Close(file: FileAccess) -> void:
    if file:
        file.close()

static func ReadAllText(file: FileAccess) -> String:
    if file:
        return file.get_as_text()
    return ""

static func WriteAllText(file: FileAccess, text: String) -> void:
    if file:
        file.store_string(text)
