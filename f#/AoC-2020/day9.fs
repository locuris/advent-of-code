module day9

let initialState (lines: string array) : bool array3d =
    let size = lines.Length
    let middle = (size - 1) / 2
    let cube: bool array3d = Array3D.init size size size (fun _ _ _ -> false)

    for x in 0 .. size - 1 do
        for y in 0 .. size - 1 do
            if lines.[x].[y] = '#' then
                cube.[x, y, middle] <- true

    cube

let checkNeighbours (cube: bool array3d) (x: int) (y: int) (z: int) : int =
    let offsets = [ -1; 0; 1 ]

    let neighbours =
        [ for dx in offsets do
              for dy in offsets do
                  for dz in offsets do
                      if dx <> 0 || dy <> 0 || dz <> 0 then
                          yield x + dx, y + dy, z + dz ]

    let mutable active = 0

    for x, y, z in neighbours do
        if
            x >= 0
            && x < cube.GetLength(0)
            && y >= 0
            && y < cube.GetLength(1)
            && z >= 0
            && z < cube.GetLength(2)
            && cube.[x, y, z]
        then
            active <- active + 1

    active

let executeCycle (cube: bool array3d) : bool array3d =
    cube
    |> Array3D.mapi (fun x y z state ->
        let activeNeighbours = checkNeighbours cube x y z

        if state then
            activeNeighbours = 2 || activeNeighbours = 3
        else
            activeNeighbours = 3)


let executeCycles (cube: bool array3d) (cycles: int) : bool array3d =
    let mutable cube = cube

    for _ in 1 .. cycles do
        let size = cube.GetLength(0)
        let newSize = size + 2
        let newCube: bool array3d = Array3D.init newSize newSize newSize (fun _ _ _ -> false)
        
        for x in 0 .. size - 1 do
            for y in 0 .. size - 1 do
                for z in 0 .. size - 1 do
                    newCube.[x + 1, y + 1, z + 1] <- cube.[x, y, z]
        
        cube <- newCube
        cube <- executeCycle cube

    cube
    
let day9part1 (lines: string array) : string =
    let mutable cube = initialState lines
    cube <- executeCycles cube 6
    let mutable activeCubes = 0
    cube |> Array3D.map (
        fun x ->
            if x then
                activeCubes <- activeCubes + 1
                1
                else
                    0
                    ) |> ignore
    activeCubes |> string