extends Object

static func Replace(text: String, what: String, for_what: String) -> String:
    return text.replace(what, for_what)

static func Contains(text: String, what: String) -> bool:
    return text.contains(what)

static func Find(text: String, what: String) -> int:
    return text.find(what)

static func Insert(text: String, pos: int, what: String) -> String:
    return text.insert(pos, what)
