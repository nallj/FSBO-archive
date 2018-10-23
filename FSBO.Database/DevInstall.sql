
-- Create initial subscribers.
INSERT INTO Subscriber([Name], [Email], [IsActive])
VALUES
    (N'James Nall', 'nero777x@gmail.com', 1),
    (N'Jeffrey Clarke', 'jeffwclarkejr@gmail.com', 0),
    (N'Mustafa Lakadawala', 'lakadawala.mustafa@gmail.com', 0);

-- Create area types.
INSERT INTO AreaType([Name])
VALUES
    ('Postal Code'),
    ('City and State'),
    ('City, State, and Postal Code');


-- Add Zillow as a Source (should have an ID of 1).
INSERT INTO Source([HeadUri], [IsConfirmed], [AddedOn])
VALUES
    (N'http://www.zillow.com/', 1, SYSDATETIMEOFFSET()),
	--(N'https://www.trulia.com/', 1, SYSDATETIMEOFFSET());
        -- Jeffrey recommended that Trulia be ignored since it is managed by the same parent company as Zillow.
    (N'http://www.realtor.com/', 1, SYSDATETIMEOFFSET());


-- Create basic Gainesville, FL area.
INSERT INTO Area([AreaTypeId], [Value], [ApprovedOn])
VALUES
    (3, 'Gaineville, FL, 32608', SYSDATETIMEOFFSET());

-- Create entry point for "Gainesville, FL 32608" area for Zillow.
INSERT INTO SourceAreaEntry([SourceId], [AreaId], [EntryPoint])
VALUES
    (1, 1, N'gainesville-fl-32608/fsbo/');

-- Create basic target fields types.
INSERT INTO TargetFieldType([Name])
VALUES
    ('bool'), -- 1
    ('char'), -- 2
    ('string'), -- 3
    ('int'), -- 4
    ('uint'), -- 5
    ('ulong'), -- 6
    ('decimal'); -- 7

-- Create initial target fields.
INSERT INTO TargetField([Name], [FieldTypeId])
VALUES
    ('Owner, Info', 3), -- 1
    ('Address, Street Number', 5), -- 2
    ('Address, Street Name', 3), -- 3
    ('Address, Apartment Number', 3), -- 4
    ('Address, Line 2', 3), -- 5
    ('Address, City', 3), -- 6
    ('Address, State', 3), -- 7
    ('Address, Postal Code', 5), -- 8
    ('Owner, Home Phone Number', 6), -- 9
    ('Owner, Cell Phone Number', 6), -- 10
    ('Owner, Fax Number', 6), -- 11
    ('Owner, Email', 3), -- 12
    ('Asking Price', 7), -- 13
	('Bedroom Count', 5), -- 14
	('Bathroom Count', 5), -- 15
	('Area, Square Feet', 5), -- 16
	('Description', 3), -- 17
	('Facts, Bedrooms', 3), -- 18
    ('Facts, Bath', 3), -- 19
	('Facts, HOA Fee', 3), -- 20
	('Facts, Year Built', 5), -- 21
    ('Owner''s Details', 3), -- 22
	('County Appraiser''s URI', 3), -- 23
    ('Details URI', 3), -- 24
    ('Listing Age', 3) -- 25
	;

-- Create temporary fields.
/*INSERT INTO TargetField([Name], [FieldTypeId], [IsTemporaryField])
VALUES
    ('Details URI', 3, 1) -- 24
	;*/

-- Add main and details Zillow scraping templates.
INSERT INTO Template([SourceId], [Name], [IsTopLevel])
VALUES
	(1, 'Zillow Main Results Page', 1),
    (1, 'Zillow Details Page', 0);

-- Associate Zillow's main template with Zillow.
/*INSERT INTO SourceTemplate([SourceId], [TemplateId])
VALUES
    (1, 1);*/

-- CONSIDER CHOOSING A CHILD RELATIONSHIOP FOR THE DAL. CANT CONNECT TO PARENT
-- Nest Zillow's details template under its' main template.
INSERT INTO TemplateParent([TemplateId], [ParentId])
VALUES
    (2, 1);

-- Create connection from target fields to scraping templates.
INSERT INTO TemplateField([TemplateId], [TargetId], [OrderIndex])
VALUES
    -- Zillow's main template.
    (1, 3, 1), -- 1: Street Name
    (1, 6, 2), -- 2: City
    (1, 7, 3), -- 3: State
    (1, 8, 4), -- 4: Zip
    (1, 13, 5), -- 5: Asking Price
	(1, 16, 6), -- 6: Area
    (1, 25, 7), -- 7: Listing Age
    (1, 24, 8), -- 8: Details URI

    -- Zillow's details child template.
	(2, 1, 0), -- 9: Owner Info
	(2, 4, 1), -- 10: Apt #
	(2, 9, 2), -- 11: Owner Home Phone
	(2, 10, 3), -- 12: Owner Cell Phone
	(2, 11, 4), -- 13: Owner Fax
	(2, 12, 5), -- 14: Owner Email
	(2, 16, 6), -- 15: Area
	(2, 17, 7), -- 16: Description
	(2, 14, 8), -- 17: Bedroom Count

    (2, 15, 9), -- 18: Bathroom Count here
    (2, 19, 10), -- 19: Facts, Bath
	(2, 20, 11), -- 20: Facts, HOA
	(2, 21, 11), -- 21: Facts, Year Built
    (2, 22, 12), -- 22: Owner's Details
	(2, 23, 13) -- 23: County Appraiser URI
	;


-- Create basic disqualification types.
INSERT INTO DisqualificationType([Name])
VALUES
    ('Record node has class');


-- Create record disqualifications for templates.
INSERT INTO RecordDisqualification([TemplateId], [DisqualificationTypeId], [Parameters])
VALUES
    (1, 1, N'search-list-ad-native'),
    (1, 1, N'search-list-ad'),
    (1, 1, N'search-list-upsell');


-- Create basic scraping action types.
INSERT INTO ScrapeActionType([Name])
VALUES
	-- Node Navigation
    ('CSS Select'), -- 1
    ('First Descendant Node of Type'), -- 2
    ('Descendant Node of Type with Attributes and Values'), -- 3
	('Nth Descendant Node of Type'), -- 4

    -- Data Capture
    ('Select Inner Text'), -- 5
    ('Select Attribute Value'), -- 6
	('Select Inner Text (Temporary Value)'), -- 7

    -- Browser Navigation
    ('Travel to Child Template'), -- 8
    ('Temporary Travel to Derived Location'), -- 9

	-- Data Manipulation
	('Split Data'), -- 10
    ('Replace Instances Of'), -- 11
    ('Remove Outside of Borders (Exclusive)'), -- 12
    ('Trim'), -- 13

	-- Search for Specific Instance
	('Node with Inner Text Substring') -- 14
	;


-- Create the setup actions for Zillow's main scraping template.
-- UNSURE DIRECTION: Records with a NULL TemplateId will be executed last (treat it like a 9001).
INSERT INTO SetupAction([TemplateId], [ActionTypeId], [OrderIndex], [Parameters])
VALUES
    (1, 1, 1, N'#search-results ul.photo-cards'),
    (1, 2, 2, N'li');


-- Create the scraping actions for Zillow's main scraping template.
INSERT INTO ScrapeAction([TemplateFieldId], [ActionTypeId], [OrderIndex], [Parameters])
VALUES
    (1, 2, 1, N'article'), -- Address Street Name
    (1, 3, 2, N'{ Node: "span", Attributes: ["itemprop"], Values: ["streetAddress"] }'),
	(1, 5, 3, NULL),

    (2, 2, 1, N'article'), -- Address City
    (2, 3, 2, N'{ Node: "span", Attributes: ["itemprop"], Values: ["addressLocality"] }'),
	(2, 5, 3, NULL),

    (3, 2, 1, N'article'), -- Address State
    (3, 3, 2, N'{ Node: "span", Attributes: ["itemprop"], Values: ["addressRegion"] }'),
	(3, 5, 3, NULL),
	
    (4, 2, 1, N'article'), -- Address Zip
    (4, 3, 2, N'{ Node: "span", Attributes: ["itemprop"], Values: ["postalCode"] }'),
	(4, 5, 3, NULL),

    (5, 1, 1, N'span.zsg-photo-card-price'), -- Asking Price
	(5, 5, 2, NULL),

    --(, 1, 1, N'span.zsg-photo-card-info'), -- Bedroom Count
	--(, 5, 2, NULL),
	--(, 9, 3, N'{ Split: " bd", Index: 0 }'),

	--(, 1, 1, N'span.zsg-photo-card-info'), -- Bathroom Count
	--(, 5, 2, NULL),
	--(, 9, 3, N'{ Split: " bd", Index: 0 }'),

    --(6, , 1, ), -- Area, Square Feet

    (7, 1, 1, N'span.zsg-photo-card-notification'), -- Listing Age
    (7, 5, 2, NULL),

    (8, 2, 1, N'a'), -- Details URI
    (8, 6, 2, N'href'),
    (8, 8, 3, N'{ Child: 2, Source: "24", IsFromTemporaryField: false }'), -- travel to a child template, using "Details URI" data.

	--(NULL, 2, 1, N'a'), -- Details URI (temporary value)
    --(NULL, 6, 2, N'href'),
	--(NULL, 7, 3, N'detailsUri'),

    -------------------------------------------
    -------------------------------------------
	--(NULL, 8, 1, N'{ Child: 2, Source: "7" }'), -- travel to a child template, using "Details URI" data.
    -------------------------------------------
    -------------------------------------------

    (9, 1, 1, N'section#listing-provided-by'), -- Owner Info
	(9, 4, 2, N'{ Node: "div", Depth: 2 }'),
	(9, 2, 3, N'p'),

    --(10, , , ), -- Apartment Number
	--(10, , , ),

	--(11, , , ), -- Home #
	--(11, , , ),

	--(12, , , ), -- Cell #
	--(12, , , ),

	--(13, , , ), -- Fax #
	--(13, , , ),

	--(14, , , ), -- Email
	--(14, , , ),

    (15, 1, 1, N'header.zsg-content-header'), -- Area, Square Feet
	(15, 2, 2, N'h3'),
	(15, 3, 3, N'{ Node: "span", Attributes: ["class"], Values: ["addr_bbs"], Depth: 3 }'),
	(15, 5, 4, NULL),
	(15, 10, 5, N'{ Split: " sqft", Index: 0 }'),

    (16, 3, 1, N'{ Node: "div", Attributes: ["role"], Values: ["main"] }'), -- Description
	(16, 2, 2, N'section'),
	(16, 1, 3, N'.zsg-content-item'),
	(16, 5, 4, NULL),

	(17, 1, 1, N'header.zsg-content-header'), -- Bedroom Count
	(17, 2, 2, N'h3'),
	(17, 3, 3, N'{ Node: "span", Attributes: ["class"], Values: ["addr_bbs"] }'),
	(17, 5, 4, NULL),
	(17, 10, 5, N'{ Split: " bed", Index: 0 }'),

	(18, 1, 1, N'header.zsg-content-header'), -- Bathroom Count
	(18, 2, 2, N'h3'),
	(18, 3, 3, N'{ Node: "span", Attributes: ["class"], Values: ["addr_bbs"], Depth: 2 }'),
	(18, 5, 4, NULL),
	(18, 10, 5, N'{ Split: " bath", Index: 0 }'),

	(19, 1, 1, N'.hdp-facts'), -- Facts, Baths
	(19, 14, 2, N'{ Node: "li", Substring: "Baths:" }'),
	(19, 10, 3, N'{ Split: "Baths: ", Index: 1 }'),

	(20, 1, 1, N'.hdp-facts'), -- Facts, HOA Fee
	(20, 14, 2, N'{ Node: "li", Substring: "HOA Fee:" }'),
	(20, 10, 3, N'{ Split: "HOA Fee: ", Index: 1 }'),

	(21, 1, 1, N'.hdp-facts'), -- Facts, Year Built
	(21, 14, 2, N'{ Node: "li", Substring: "Built" }'),
	(21, 10, 3, N'{ Split: "Built in ", Index: 1 }'),

    --(22, 1, 1, N'section#listing-provided-by-module'), -- Owner's Details
    --(22, 3, 2, N'{ Node: "section", Attributes: ["id"], Values: ["listing-provided-by"] }'),
    --(22, 3, 3, N'{ Node: "p", Attributes: ["class"], Values: ["notranslate"] }'),

    (22, 14, 1, N'{ Node: "script", Substring: "divId:\"listing-provided-by-module\"" }'), -- Owner's Details
    (22, 5, 2, NULL),
    (22, 12, 3, N'{ Left: "asyncLoader.load({ajaxURL:\"", Right: "\",jsModule:\"z-complaint-manager-async-block\",phaseType:\"scroll\",divId" }'),
    (22, 9, 4, NULL),
    (22, 5, 5, NULL),
    (22, 12, 6, N'{ Left: "Listing Provided by Owner", Right: "Report problem with listing" }'),
    (22, 13, 7, NULL),

	(23, 3, 1, N'{ Node: "a", Attributes: ["data-za-action"], Values: ["CountyLink"] }'), -- County Appraiser's URI
	(23, 6, 2, N'href'),
    (23, 11, 3, N'{ Replace: ":443" }'),
    (23, 11, 3, N'{ Replace: "&amp;", With: "&" }')
	;