module day5

open System
open System.ComponentModel
open Common.Input

type Range =
    struct
        val Start: int64
        val End: int64
        new(start: int64, end_: int64) = {
            Start = start
            End = end_ - 1L
        }
        
        member this.Within(value: int64) =
            value >= this.Start && value <= this.End
            
        member this.Within(value: Range) =
            value.Start <= this.End && value.End >= this.Start
            
        member this.Overlap(other: Range) =
            if other.Start > this.End || other.End < this.Start then
                other
            else
                let start = max this.Start other.Start
                let end_ = min this.End other.End
                Range(start, end_)
    end
    
type Seeds =
    struct
        val Ranges: Range array
        new(ranges: Range array) = { Ranges = ranges }
        
        member this.Contains(seed: int64) =
            this.Ranges |> Array.exists (fun r -> r.Within(seed))
                
    end    

type MapRange =
    struct
        val DestinationRange: Range
        val SourceRange: Range
        new(destRangeStart: int64, sourceRangeStart: int64, rangeLength: int64) = {
            DestinationRange = Range(destRangeStart, destRangeStart + rangeLength)
            SourceRange = Range(sourceRangeStart, sourceRangeStart + rangeLength)
                    }
        
        member this.GetDestination(source: int64) =
            if this.SourceRange.Within source then
                this.DestinationRange.Start + (source - this.SourceRange.Start)
            else
                source
                
        member this.GetDestination(source: Range) =
            if this.SourceRange.Within source then
                let sourceRange = this.SourceRange.Overlap source
                let start = sourceRange.Start - this.SourceRange.Start
                let end_ = this.SourceRange.End - sourceRange.End                
                let range = Range(this.DestinationRange.Start + start, this.DestinationRange.End - end_)
                range
            else
                source
            
    end

type SeedMap =
    struct
        val Source: string
        val Destination: string
        val Ranges : MapRange array
        new(source: string, dest: string, ranges: MapRange array) = {
            Source = source
            Destination = dest
            Ranges = ranges
        }        
        member this.GetDestination(source: int64) =
            let mutable dest = source
            for range in this.Ranges do
                let tempDest = range.GetDestination(source)
                if not (tempDest = source) then
                    dest <- tempDest
            dest
                                   
        member this.GetDestinations(sources: Range array) =
            let ranges = this.Ranges
            sources |> Array.collect (fun source ->
                let destinations = ranges
                                   |> Array.map (fun mr -> mr.GetDestination(source))
                                   |> Array.filter (fun r -> not (r.Start = source.Start && r.End = source.End))
                if destinations.Length = 0 then
                    [|source|]
                    else destinations)
            |> Array.distinct
            
    end
    
let getSourceAndDestination(groups: string array): string * string =
    groups |> Seq.head, groups |> Seq.last
    
let createSeedMaps(groups: string array list) =
    groups |> List.removeAt 0 |> List.map (fun mapLines ->
        let source, destination = mapLines |> Array.item 0 |> GetGroupsAsStringArray @"(\w+)-\w+-(\w+)" |> getSourceAndDestination
        let ranges = mapLines |> Array.removeAt 0 |> Array.map (fun line ->
            let rangeNumbers = GetMatchesAsStringArray @"\d+" line |> Seq.map Int64.Parse |> Seq.toArray
            MapRange(rangeNumbers[0], rangeNumbers[1], rangeNumbers[2]))
        SeedMap(source, destination, ranges)) |> List.toArray
    
let getSeedRanges(seeds: int64 array) =
    let results = ResizeArray<Range>()
    for n in 0..(seeds.Length / 2) - 1 do
        let rn = n * 2
        let start = seeds[rn]
        let _end = start + seeds[rn+1]
        results.Add(Range(start, _end))
    Seeds(results.ToArray())
    
let part1(lines: string array) =
    let groups = lines |> GetLinesGroupedByNewLine
    let seeds = groups |> List.item 0 |> Array.item 0 |> GetMatchesAsStringArray @"\d+" |> Array.map Int64.Parse |> Seq.toArray
    let seedMaps = createSeedMaps groups
    seeds |> Array.map (fun s ->
        let mutable seed = s
        seedMaps |> Array.iter (fun map -> seed <- map.GetDestination(seed))
        seed) |> Array.min |> string
    
let part2(lines: string array) =
    let groups = lines |> GetLinesGroupedByNewLine
    let seeds = groups |> List.item 0 |> Array.item 0 |> GetMatchesAsStringArray @"\d+" |> Array.map Int64.Parse |> Seq.toArray |> getSeedRanges
    let seedMaps = createSeedMaps groups
    seedMaps |> Array.fold (fun r sm -> sm.GetDestinations(r)) seeds.Ranges
    |> Array.distinct
    |> Array.map (fun sr -> sr.Start)
    |> Array.min |> string