#pragma once

#include "../PCore/Common.h"

#include "EntityComponent.h"

namespace HRZ
{

extern const RTTI *RTTI_ItemDescriptionComponent;
extern const RTTI *RTTI_ItemDescriptionComponentResource;

class ItemPriceInfo;
class LocalizedTextResource;
class LootItemDescriptionResource;
class MovieResource;
class StatsDisplayResource;
class UITexture;

class ItemDescriptionComponentResource : public EntityComponentResource
{
public:
	static inline auto& TypeInfo = RTTI_ItemDescriptionComponentResource;

	Ref<LocalizedTextResource> m_LocalizedItemName;					// 0x28
	Ref<LocalizedTextResource> m_LocalizedItemDescription;			// 0x30
	Ref<ItemPriceInfo> m_PriceInfo;									// 0x38
	int m_ItemWeight;												// 0x40
	Ref<UITexture> m_UIIconTexture;									// 0x48
	Ref<UITexture> m_UIIconInactiveTexture;							// 0x50
	UUIDRef<UITexture> m_PreviewTexture;							// 0x58
	Ref<LootItemDescriptionResource> m_LootItemDescriptionResource;	// 0x68
	Ref<StatsDisplayResource> m_StatsDisplayResource;				// 0x70
	Ref<MovieResource> m_Movie;										// 0x78

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~ItemDescriptionComponentResource() override;		// 1
	virtual const RTTI *GetComponentRTTI() const override;		// 18
	virtual const RTTI *GetComponentRepRTTI() const override;	// 19
};
assert_size(ItemDescriptionComponentResource, 0x80);

class ItemDescriptionComponent : public EntityComponent
{
public:
	static inline auto& TypeInfo = RTTI_ItemDescriptionComponent;

	virtual const RTTI *GetRTTI() const override;				// 0
	virtual ~ItemDescriptionComponent() override;				// 1
	virtual const RTTI *GetComponentRepRTTI() const override;	// 4
};
assert_size(ItemDescriptionComponent, 0x58);

}