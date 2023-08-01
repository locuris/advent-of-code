module day4

open System

let passportFields: Set<string> = Set.ofList ["byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"; "cid"]
let requiredFields: Set<string> = Set.ofList ["byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"]

let day4part1 (lines: string array): string =
    //let passportStrings: ResizeArray<List<string>> = ResizeArray []
    
    let passports = ResizeArray<string> []
    
    let mutable currentPassport = ""
    for line in lines do
        if line = "" then
            passports.Add(currentPassport)
            currentPassport <- ""
        else 
            currentPassport <- currentPassport + " " + line
            
    passports.Add(currentPassport)
    
    let mutable validPassports = 0
    for passport in passports do
        let containsField (field: string) =
            passport.Contains(field)
        if Set.forall containsField requiredFields then
            validPassports <- validPassports + 1
        else ()   
        
    validPassports.ToString()
    
            
                
    
        
        
    