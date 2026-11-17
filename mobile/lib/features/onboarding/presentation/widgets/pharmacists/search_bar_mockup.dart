import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class SearchBarMockup extends StatelessWidget {
  const SearchBarMockup({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Container(
      height: 44.h,
      padding: EdgeInsetsDirectional.symmetric(
        horizontal: AppDimensions.spacing16.w,
      ),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(22.r),
      ),
      child: Row(
        children: [
          Icon(Icons.search_rounded, size: 18.sp, color: AppColors.muted),
          SizedBox(width: AppDimensions.spacing8.w),
          Text(
            s.pharmacistMockupSearchHint,
            style: AppTextStyles.label14MutedRegular,
          ),
        ],
      ),
    );
  }
}