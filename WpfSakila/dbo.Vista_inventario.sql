CREATE VIEW [dbo].[Vista_Arriendo]
	as select 
fm.film_id,fm.title as Nombre_peliculafm,fm.rental_duration,fm.rental_rate,fm.replacement_cost,inv.inventory_id

from  
film fm,
inventory inv


where
fm.film_id = inv.film_id