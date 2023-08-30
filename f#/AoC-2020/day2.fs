module day2

let getPasswordMap(lines: string array): Map<string, char * int * int> =
    let mutable passwords = Map.empty<string, char * int * int>
    
    for line in lines do
        let parts = line.Split(' ')
        let range = parts.[0].Split('-')
        let min = int range.[0]
        let max = int range.[1]
        let letter = parts.[1].[0]
        let passwordFromParts = parts.[2]
        passwords <- passwords.Add(passwordFromParts, (letter, min, max))
    
    passwords

let day2part1 (lines: string array) =
    let mutable passwords = getPasswordMap(lines)
    
    let mutable valid = 0
    for password in passwords.Keys do
        let mutable count = 0
        let policyLetter, min, max = passwords.[password]
        for passwordLetter in password do
            if passwordLetter.Equals(policyLetter) then
                count <- count + 1
        if count >= min && count <= max then
            valid <- valid + 1
    
    valid.ToString()
    
let day2part2 (lines: string array) =
    let mutable passwords = getPasswordMap(lines)
    
    let mutable valid = 0
    for password in passwords.Keys do
        let mutable count = 0
        let policyLetter, min, max = passwords.[password]
        for i in 1..password.Length do
            let idx = i - 1
            if password[idx] = policyLetter && (i = min || i = max) then
                count <- count + 1
        if count = 1 then
            valid <- valid + 1
    valid.ToString()
            
        
                