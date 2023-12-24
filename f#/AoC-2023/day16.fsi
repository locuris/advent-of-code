// day16.fsi
module day16


type Point =
    { x : int; y : int }
    static member (+) : Point * Point -> Point

type MirrorAngle = 
    | Forward = '/'
    | Back = '\\'

type SplitAxis =
    | Horizontal = '-'
    | Vertical = '|'
    
type SplitResult =
    | Continue = 0 
    | Split = 1

type Direction =
    | Up 
    | Down 
    | Left 
    | Right
    member Point : Point
    member ReflectedPoint : MirrorAngle -> Point
    member SplitResult : SplitResult -> D

    
