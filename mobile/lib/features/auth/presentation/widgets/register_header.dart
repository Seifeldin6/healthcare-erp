import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class RegisterHeader extends StatelessWidget {
  final int currentIndex;
  final String titleText;
  final String subTitleText;

  const RegisterHeader({
    super.key,
    required this.currentIndex,
    required this.titleText,
    required this.subTitleText,
  });

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      mainAxisSize: MainAxisSize.min,
      children: [
        Text(titleText, style: AppTextStyles.display26InkExtraBold),
        SizedBox(height: 4.h),
        Text.rich(
          TextSpan(
            children: [
              TextSpan(
                text: s.stepOfTotal(
                  currentIndex + 1,
                  4,
                ),
                style: AppTextStyles.body14PrimaryBold,
              ),
              TextSpan(
                text: ' · $subTitleText',
                style: AppTextStyles.body14MutedRegular,
              ),
            ],
          ),
        ),
        SizedBox(height: 12.h),
        Row(
          children: List.generate(4, (index) {
            final isActive = index <= currentIndex;
            return Expanded(
              child: Container(
                height: 4.h,
                margin: EdgeInsetsDirectional.only(end: index < 3 ? 8.w : 0),
                decoration: BoxDecoration(
                  color: isActive ? AppColors.primary : AppColors.line,
                  borderRadius: BorderRadius.circular(2.r),
                ),
              ),
            );
          }),
        ),
      ],
    );
  }
}