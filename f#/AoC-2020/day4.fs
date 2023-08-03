module day4

open System
open System.Collections.Generic

let passportFields: Set<string> = Set.ofList ["byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"; "cid"]
let requiredFields: Set<string> = Set.ofList ["byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"]

let getPassports (lines: string array): Map<string, bool> =
    let mutable passports: Map<string, bool> = Map.empty
    
    let mutable currentPassport = ""
    for line in lines do
        if line = "" then
            passports <- passports.Add(currentPassport, false)
            currentPassport <- ""
        else
            currentPassport <- currentPassport + " " + line
            
    passports <- passports.Add(currentPassport, false)
    passports

let day4part1 (lines: string array): string =
    let passports = getPassports lines    
        
    let mutable validPassports = 0
    for passport in passports.Keys do
        let containsField (field: string) =
            passport.Contains(field)
        if Set.forall containsField requiredFields then
            validPassports <- validPassports + 1
        else ()   
        
    validPassports.ToString()
   
let validateBirthYear (byr: string): bool =
    let byrInt = int byr
    byrInt >= 1920 && byrInt <= 2002
    
let validateIssueYear (iyr: string): bool =
    let iyrInt = int iyr
    iyrInt >= 2010 && iyrInt <= 2020
    
let validateExpirationYear (eyr: string): bool =
    let eyrInt = int eyr
    eyrInt >= 2020 && eyrInt <= 2030
    
let validateHeight (hgt: string): bool =
    if hgt.Length < 3 then
        false
    else
        let length = hgt.Length - 2
        let value = hgt.Substring(0, length) |> int
        match hgt.Substring(hgt.Length - 2) with
        | "cm" ->
            value >= 150 && value <= 193
        | "in" ->
            value >= 59 && value <= 76
        | _ -> false
        
let validateHairColor (hcl: string): bool =
    if hcl.Length <> 7 then
        false
    elif hcl.[0] <> '#' then
        false
    else
        let color = hcl.Substring(1)
        String.forall (fun c -> Char.IsDigit(c) || (c >= 'a' && c <= 'f')) color
        
let validateEyeColor (ecl: string): bool =
    let validColors = Set.ofList ["amb"; "blu"; "brn"; "gry"; "grn"; "hzl"; "oth"]
    Set.contains ecl validColors
    
let validatePassportId (pid: string): bool =
    if pid.Length <> 9 then
        false
    else
        String.forall Char.IsDigit pid
    
let day4part2 (lines: string array): string =
    let passportStrings = getPassports lines
    
    let mutable passports: Map<string, Map<string, string>> = Map.empty
    
    for passportString in passportStrings.Keys do
        let fields = passportString.Split(' ')
        let mutable passportValues = Map.empty
        for field in fields do
            let keyAndValue = field.Split(':')
            if keyAndValue.Length = 2 then
                passportValues <- passportValues.Add(keyAndValue.[0], keyAndValue.[1])
                      
        passports <- passports.Add(passportString, passportValues)
        
    let mutable validPassports = 0
    
    for KeyValue(passport, details) in passports do
        let containsField (field: string) =
            passport.Contains(field)
        if Set.forall containsField requiredFields then
            let mutable valid = true
            for KeyValue(detailKey, detailValue) in details do
                if valid then
                    match detailKey with
                    | "byr" -> valid <- validateBirthYear detailValue
                    | "iyr" -> valid <- validateIssueYear detailValue
                    | "eyr" -> valid <- validateExpirationYear detailValue
                    | "hgt" -> valid <- validateHeight detailValue
                    | "hcl" -> valid <- validateHairColor detailValue
                    | "ecl" -> valid <- validateEyeColor detailValue
                    | "pid" -> valid <- validatePassportId detailValue
                    | "cid" -> valid <- true
                    | _ -> valid <- false
                else ()
            if valid then
               validPassports <- validPassports + 1
            else ()
                
    validPassports.ToString()
        