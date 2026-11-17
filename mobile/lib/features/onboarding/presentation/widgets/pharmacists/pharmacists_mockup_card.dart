import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/pharmacists/build_filter_chip.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/pharmacists/build_pharmacist_item.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/pharmacists/build_skeleton_card.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/pharmacists/search_bar_mockup.dart';
import 'package:healthcare_app/generated/l10n.dart';

class PharmacistsMockupCard extends StatelessWidget {
  const PharmacistsMockupCard({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return ClipRRect(
      borderRadius: BorderRadius.circular(28.r),
      child: Container(
        width: double.infinity,
        color: AppColors.primaryTint,
        padding: EdgeInsetsDirectional.fromSTEB(
          AppDimensions.spacing16.w,
          AppDimensions.spacing16.h,
          AppDimensions.spacing16.w,
          0,
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const SearchBarMockup(),
            SizedBox(height: AppDimensions.spacing12.h),
            Row(
              children: [
                BuildFilterChip(
                  label: s.pharmacistMockupFilterAll,
                  isSelected: true,
                ),
                SizedBox(width: AppDimensions.spacing8.w),
                BuildFilterChip(
                  label: s.pharmacistMockupFilterClinical,
                  isSelected: false,
                ),
                SizedBox(width: AppDimensions.spacing8.w),
                BuildFilterChip(
                  label: s.pharmacistMockupFilterHerbal,
                  isSelected: false,
                ),
              ],
            ),
            SizedBox(height: AppDimensions.spacing12.h),
            BuildPharmacistItem(
              initials: s.pharmacistMockupInitials1,
              avatarBg: AppColors.onPrimarySoft,
              avatarTextStyle: AppTextStyles.body16PrimaryBold,
              name: s.pharmacistMockupName1,
              specialty: s.pharmacistMockupSpecialty1,
              price: s.pharmacistMockupPrice1,
            ),
            SizedBox(height: AppDimensions.spacing12.h),
            BuildPharmacistItem(
              initials: s.pharmacistMockupInitials2,
              avatarBg: AppColors.warmTint,
              avatarTextStyle: AppTextStyles.body16WarmInkBold,
              name: s.pharmacistMockupName2,
              specialty: s.pharmacistMockupSpecialty2,
              price: s.pharmacistMockupPrice2,
            ),
            SizedBox(height: AppDimensions.spacing12.h),
            ClipRect(
              child: Align(
                alignment: Alignment.topCenter,
                heightFactor: 0.7,
                child: const BuildSkeletonCard(),
              ),
            ),
          ],
        ),
      ),
    );
  }
}